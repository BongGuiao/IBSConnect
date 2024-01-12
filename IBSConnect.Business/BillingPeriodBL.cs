using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using IBSConnect.Data.Models;

namespace IBSConnect.Business;

public class BillingPeriodBL : IBillingPeriodBL
{
    private readonly IBSConnectDataContext _dataContext;

    public BillingPeriodBL(IBSConnectDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<BillingPeriodViewModel>> GetAllAsync()
    {
        return await _dataContext.BillingPeriods.Select(b => new BillingPeriodViewModel()
        {
            Id = b.Id,
            Name = b.Name,
            CreatedDate = b.CreatedDate
        }).ToListAsync();
    }

    public async Task CreateAsync(CreateBillingRequest request)
    {
        var entity = _dataContext.BillingPeriods.Add(new BillingPeriod()
        {
            Name = request.Name,
            CreatedDate = DateTime.Now,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        });

        await _dataContext.SaveChangesAsync();

        var id = entity.Entity.Id;

        var currentBillingPeriod = await _dataContext.Settings.Where(s => s.Name == "CurrentBillingPeriod").SingleAsync();

        currentBillingPeriod.Value = id.ToString();

        await _dataContext.SaveChangesAsync();

    }


    public async Task<BillingPeriodViewModel> GetCurrentPeriodAsync()
    {
        var currentBillingPeriodSetting = await _dataContext.Settings.Where(s => s.Name == "CurrentBillingPeriod").SingleAsync();

        return await _dataContext.BillingPeriods
            .Where(b => b.Id == int.Parse(currentBillingPeriodSetting.Value))
            .Select(b => new BillingPeriodViewModel()
            {
                Id = b.Id,
                Name = b.Name,
                CreatedDate = b.CreatedDate
            }).SingleOrDefaultAsync();
    }

    public async Task UpdateAsync(int id, UpdateBillingRequest request)
    {
        var billingPeriod = await _dataContext.BillingPeriods.Where(b => b.Id == id).SingleOrDefaultAsync();

        billingPeriod.Name = request.Name;
        billingPeriod.StartDate = request.StartDate;
        billingPeriod.EndDate = request.EndDate;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var billingPeriod = await _dataContext.BillingPeriods.Where(b => b.Id == id).SingleOrDefaultAsync();

        _dataContext.Remove(billingPeriod);

        await _dataContext.SaveChangesAsync();
    }
}