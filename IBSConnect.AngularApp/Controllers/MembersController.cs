using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using System;

namespace IBSConnect.AngularApp.Controllers;

public class CreditMinutesRequest
{
    public int Minutes { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class MembersController : AuthorizedUserControllerBase
{
    private readonly IMemberBL _memberBl;
    private readonly IMemberSessionBL _memberSessionBl;
    private readonly IMetaDataBL _metaDataBl;


    public MembersController(IMemberBL memberBl, IMemberSessionBL memberSessionBl, IMetaDataBL metaDataBl)
    {
        _memberBl = memberBl;
        _memberSessionBl = memberSessionBl;
        _metaDataBl = metaDataBl;

    }

    [HttpPost("authenticate")]
    public async Task<AuthenticateResponse> Authenticate(MemberAuthenticateRequest model)
    {
        var response = await _memberBl.AuthenticateAsync(model);

        var currentSession = await _memberSessionBl.GetCurrentSessionAsync(response.Id);

        if (currentSession != null)
        {
            if (currentSession.IsWithArrears)
            {
                string msg = "Hi, It seems you have an unsettled balance of "+ currentSession.Amount.ToString() +"  from Previous Billing Period. Kindly settle this with Library Admin. Thanks.";
                throw new ApplicationException(msg);
            }
        }
        if (currentSession == null)
        {
            await _memberSessionBl.StartSessionAsync(response.Id, model.Applications, model.UnitArea);
        }

        return response;
    }

    [HttpGet("{id}")]
    public async Task<MemberViewModel> Get(int id)
    {
        return await _memberBl.GetByIdAsync(id);
    }

    [HttpGet("image")]
    public async Task<ActionResult> GetImage([FromQuery] string filename)
    {
        var (stream, mimeType) = await _memberBl.GetImageAsync(filename);

        return new FileStreamResult(stream, mimeType);

    }

    [Authorize(Roles.Administrator)]
    [HttpPost]
    public async Task Create(CreateMemberRequest user)
    {
        await _memberBl.CreateAsync(user);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("{id}")]
    public async Task Update(int id, [FromBody] UpdateMemberRequest user)
    {
        await _memberBl.UpdateAsync(id, user);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _memberBl.DeleteAsync(id, CurrentIdentity.Id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost("search")]
    public async Task<QueryResult<MemberListItemModel>> GetAll(FilterRequest request)
    {
        return await _memberBl.GetAllAsync(request);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("{id}/credit")]
    public async Task CreditMinutes(int id, CreditMinutesRequest request)
    {
        await _memberBl.CreditMinutesAsync(id, request.Minutes);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet("{id}/sessions")]
    public async Task<IEnumerable<SessionViewModel>> GetSessions(int id)
    {
        return await _memberBl.GetSessionsAsync(id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("{id}/sessions")]
    public async Task UpdateSession(int id, [FromBody] SessionViewModel session)
    {
        await _memberBl.UpdateSessionAsync(id, session);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet("{id}/bill")]
    public async Task<MemberBillViewModel> GetBill(int id)
    {
        return await _memberBl.GetBillAsync(id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost("billing")]
    public async Task<QueryResult<MemberBillViewModel>> GetBilling(BillingFilterRequest request)
    {
        return await _memberBl.GetBillingAsync(request);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("upload")]
    public async Task<ImportResult> UploadMembers()
    {
        var file = Request.Form.Files[0];

        await using (var stream = new MemoryStream())
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
            await file.CopyToAsync(stream);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var reader = new ExcelReader(_metaDataBl);
            var members = await reader.ReadAsync(stream);
            if (reader.Errors.Any())
            {
                var messages = new List<string>();
                foreach (var error in reader.Errors)
                {
                    messages.Add($"Error at row {error.Row} in column {error.Column}: {error.Message}");
                }

                throw new ValidationException(messages);
            }

            var result = await _memberBl.ImportMembersAsync(members);


            return result;
        }
    }

    [Authorize(Roles.Administrator)]
    [HttpGet("{id}/payments")]
    public async Task<IEnumerable<PaymentViewModel>> GetPayments(int id)
    {
        return await _memberBl.GetPaymentsAsync(id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost("{id}/payments")]
    public async Task AddPayment(int id, CreatePaymentRequest request)
    {
        await _memberBl.AddPaymentAsync(id, request.Amount, CurrentIdentity.Id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost("{id}/paymentarrears")]
    public async Task AddPaymentArrears(int id, CreatePaymentArrearsRequest request)
    {
        await _memberBl.AddPaymentArrearsAsync(id, request.Amount, CurrentIdentity.Id);
    }
    [Authorize(Roles.Administrator)]
    [HttpGet("sessions/active")]
    public async Task<IEnumerable<SessionViewModel>> GetActiveSessions()
    {
        return await _memberBl.GetActiveSessionsAsync();
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("sessions/active/close")]
    public async Task CloseActiveSessions()
    { 
        await _memberBl.CloseActiveSessionsAsync();
    }

    [Authorize(Roles.Administrator)]
    [HttpGet("{id}/paymentarrears")]
    public async Task<IEnumerable<PaymentArrearsViewModel>> GetPaymentArrears(int id)
    {
        return await _memberBl.GetPaymentArrearsAsync(id);
    }
    [Authorize(Roles.Administrator)]
    [HttpGet("{id}/totalarrears")]
    public async Task<IEnumerable<IBSTranHistoryView>> GetTotalArrears(int id)
    {
        return await _memberBl.GetTotalArrearsAsync(id);
    }

}