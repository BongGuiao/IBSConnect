using System.IO;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface IReportsBL
{
    Task<Stream> GetOutstandingBillsReportAsync();
    Task<Stream> UsageByCollegeAsync(DateRangeRequest request);
    Task<Stream> UsageByUnitAreaAsync(DateRangeRequest request);
    Task<Stream> UsageByDemographicsAsync(DateRangeRequest request);
    Task<Stream> UsageByDemographicApplicationsAsync(DateRangeRequest request);
}