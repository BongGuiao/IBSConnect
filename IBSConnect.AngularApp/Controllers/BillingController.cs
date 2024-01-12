using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSConnect.AngularApp.Controllers;


[Route("api/[controller]")]
public class BillingController : AuthorizedUserControllerBase
{
    private IBillingPeriodBL _billingPeriodBl;

    public BillingController(IBillingPeriodBL billingPeriodBl)
    {
        _billingPeriodBl = billingPeriodBl;
    }

    [Authorize(Roles.Administrator)]
    [HttpPost]
    public async Task CreateBillingPeriod([FromBody] CreateBillingRequest request)
    {
        await _billingPeriodBl.CreateAsync(request);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("{id}")]
    public async Task UpdateBillingPeriod(int id, [FromBody] UpdateBillingRequest request)
    {
        await _billingPeriodBl.UpdateAsync(id, request);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("{id}")]
    public async Task UpdateBillingPeriod(int id)
    {
        await _billingPeriodBl.DeleteAsync(id);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet]
    public async Task<IEnumerable<BillingPeriodViewModel>> GetAll()
    {
        return await _billingPeriodBl.GetAllAsync();
    }

    [Authorize(Roles.Administrator)]
    [HttpGet]
    public async Task<IEnumerable<BillingPeriodViewModel>> GetCurrentBillingMembers()
    {
        return await _billingPeriodBl.GetAllAsync();
    }

}