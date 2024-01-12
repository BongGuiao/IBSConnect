using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IBSConnect.Business.Models;
using IBSConnect.Data.Models;

namespace IBSConnect.Business;

public interface IMemberBL
{
    Task<AuthenticateResponse> AuthenticateAsync(MemberAuthenticateRequest model);
    Task<ImportResult> ImportMembersAsync(IEnumerable<Member> members);
    Task<QueryResult<MemberListItemModel>> GetAllAsync(FilterRequest filter);
    Task<MemberViewModel> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateMemberRequest member);
    Task UpdateAsync(int id, UpdateMemberRequest user);
    Task DeleteAsync(int id, int userId);
    Task CreditMinutesAsync(int id, int minutes);
    Task<(Stream, string)> GetImageAsync(string filename);
    Task<IEnumerable<SessionViewModel>> GetSessionsAsync(int id);
    Task<IEnumerable<SessionViewModel>> GetActiveSessionsAsync();
    Task CloseActiveSessionsAsync();
    Task UpdateSessionAsync(int id, SessionViewModel session);
    Task<MemberBillViewModel> GetBillAsync(int id);
    Task<QueryResult<MemberBillViewModel>> GetBillingAsync(BillingFilterRequest filter);
    Task<IEnumerable<PaymentViewModel>> GetPaymentsAsync(int id);
    Task AddPaymentAsync(int id, decimal amount, int userId);
    Task AddPaymentArrearsAsync(int id, decimal amount, int userId);
    Task<IEnumerable<PaymentArrearsViewModel>> GetPaymentArrearsAsync(int id);
    Task<IEnumerable<IBSTranHistoryView>> GetTotalArrearsAsync(int id);


}