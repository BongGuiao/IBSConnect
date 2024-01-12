using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface IBillingPeriodBL
{
    Task<IEnumerable<BillingPeriodViewModel>> GetAllAsync();
    Task CreateAsync(CreateBillingRequest request);
    Task UpdateAsync(int id, UpdateBillingRequest request);
    Task DeleteAsync(int id);
}