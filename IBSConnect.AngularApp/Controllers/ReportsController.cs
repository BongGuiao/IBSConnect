using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;

namespace IBSConnect.AngularApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : AuthorizedUserControllerBase
{
    private readonly IReportsBL _reportsBl;
    private const string xlsxMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ReportsController(IReportsBL reportsBl)
    {
        _reportsBl = reportsBl;
    }

    [HttpPost("bills/outstanding")]
    public async Task<FileStreamResult> OutstandingBills()
    {

        var stream = await _reportsBl.GetOutstandingBillsReportAsync();

        return new FileStreamResult(stream, xlsxMime)
        {
            FileDownloadName = $"Outstanding Bills.xlsx"
        };
    }

    [HttpPost("usage/bycollege")]
    public async Task<FileStreamResult> UsageByCollege(DateRangeRequest request)
    {

        var stream = await _reportsBl.UsageByCollegeAsync(request);

        return new FileStreamResult(stream, xlsxMime)
        {
            FileDownloadName = $"Usage By College.xlsx"
        };
    }

    [HttpPost("usage/byunitarea")]
    public async Task<FileStreamResult> UsageByUnitArea(DateRangeRequest request)
    {

        var stream = await _reportsBl.UsageByUnitAreaAsync(request);

        return new FileStreamResult(stream, xlsxMime)
        {
            FileDownloadName = $"Usage By UnitArea.xlsx"
        };
    }

    [HttpPost("usage/bydemographics")]
    public async Task<FileStreamResult> UsageByDemographics(DateRangeRequest request)
    {

        var stream = await _reportsBl.UsageByDemographicsAsync(request);

        return new FileStreamResult(stream, xlsxMime)
        {
            FileDownloadName = $"Usage By Demographics.xlsx"
        };
    }


    [HttpPost("usage/bydemographicapplications")]
    public async Task<FileStreamResult> UsageByDemmographicApplications(DateRangeRequest request)
    {

        var stream = await _reportsBl.UsageByDemographicApplicationsAsync(request);

        return new FileStreamResult(stream, xlsxMime)
        {
            FileDownloadName = $"Usage By Demographic and Applications.xlsx"
        };
    }
}