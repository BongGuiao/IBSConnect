using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSConnect.AngularApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : AuthorizedUserControllerBase
{
    private ISettingBL _settingBl;

    public SettingsController(ISettingBL settingBl)
    {
        _settingBl = settingBl;
    }

    [Authorize(Roles.Administrator)]
    [HttpPut]
    public async Task UpdateSettings([FromBody] IEnumerable<SettingViewModel> settings)
    {
        await _settingBl.UpdateSettingsAsync(settings);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet]
    public async Task<IEnumerable<SettingViewModel>> GetSettings()
    {
        return await _settingBl.GetSettings();
    }
    [Authorize(Roles.Administrator)]
    [HttpPut("InitializeAllottedTime")]
    public void InitializeAllottedTime(string semesterName)
    {
        _settingBl.InitializePrevisousTransaction(semesterName);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost("resethistory")]
    public async Task<int> Resethistory(IBSResetHistoryView request)
    {
        var result = await _settingBl.ResetHistoryTransaction(request);
        return result;

    }

}