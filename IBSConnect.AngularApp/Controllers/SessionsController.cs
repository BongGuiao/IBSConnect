using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSConnect.AngularApp.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AdminController : AuthorizedUserControllerBase
//{
//    private IMemberSessionBL _memberSessionBl;

//    public AdminController(IMemberSessionBL memberSessionBl)
//    {
//        _memberSessionBl = memberSessionBl;
//    }
//}


[ApiController]
[Route("api/[controller]")]
public class SessionsController : AuthorizedUserControllerBase
{
    private IMemberSessionBL _memberSessionBl;

    public SessionsController(IMemberSessionBL memberSessionBl)
    {
        _memberSessionBl = memberSessionBl;
    }

    [Authorize(Roles.Member)]
    [HttpPut("start")]
    public async Task<DateTime> StartSession(IEnumerable<int> appIds, int unitAreaId)
    {
        return await _memberSessionBl.StartSessionAsync(CurrentIdentity.Id, appIds, unitAreaId);
    }

    [Authorize(Roles.Member)]
    [HttpPut("end")]
    public async Task<DateTime> EndSession()
    {
        return await _memberSessionBl.EndSessionAsync(CurrentIdentity.Id);
    }

    [Authorize(Roles.Member)]
    [HttpPut("last")]
    public async Task<SessionViewModel> GetLastSession()
    {
        return await _memberSessionBl.GetLastSessionAsync(CurrentIdentity.Id);
    }


    [Authorize(Roles.Member)]
    [HttpGet("current")]
    public async Task<CurrentSessionViewModel> GetCurrentSession()
    {
        return await _memberSessionBl.GetCurrentSessionAsync(CurrentIdentity.Id);
    }

    [Authorize(Roles.Member)]
    [HttpGet]
    public async Task<IEnumerable<SessionViewModel>> GetSessions()
    {
        return await _memberSessionBl.GetSessionsAsync(CurrentIdentity.Id);
    }

}