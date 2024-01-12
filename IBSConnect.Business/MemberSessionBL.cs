using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using IBSConnect.Data.Models;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace IBSConnect.Business;

public class MemberSessionBL : IMemberSessionBL
{
    private readonly IBSConnectDataContext _dataContext;
    private readonly IMapper _mapper;

    private readonly AppSettings _appSettings;

    public MemberSessionBL(IBSConnectDataContext dataContext, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<DateTime> StartSessionAsync(int id, IEnumerable<int> applicationIds, int unitAreaId)
    {
        var startTime = DateTime.Now;

        var apps = (from appId in applicationIds
                    join app in _dataContext.Applications on appId equals app.Id
                    select new SessionApplication()
                    {
                        Application = app
                    }).ToList();

        var unitArea = _dataContext.UnitAreas.First(u => u.Id == unitAreaId);

        var session = new MemberSession()
        {
            MemberId = id,
            StartTime = startTime,
            Applications = apps,
            UnitArea = unitArea
        };

        _dataContext.MemberSessions.Add(session);

        await _dataContext.SaveChangesAsync();

        return startTime;
    }


    public async Task<DateTime> EndSessionAsync(int id)
    {
        var endTime = DateTime.Now;
        var session = await _dataContext.MemberSessions.Where(m => m.MemberId == id && m.EndTime == null).OrderByDescending(m => m.StartTime).FirstOrDefaultAsync();

        if (session != null)
        {
            session.EndTime = endTime;

            await _dataContext.SaveChangesAsync();

            return endTime;
        }

        throw new Exception("");
    }

    public async Task<SessionViewModel> GetLastSessionAsync(int id)
    {
        var session = await _dataContext.MemberSessions.Where(m => m.MemberId == id && m.EndTime == null).OrderByDescending(m => m.StartTime).FirstOrDefaultAsync();

        if (session == null)
        {
            return new SessionViewModel()
            {
                Id = session.Id,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };
        }

        return null;
    }

    public async Task<CurrentSessionViewModel> GetCurrentSessionAsync(int id)
    {
        var memberQuery = from m in _dataContext.Members
                          join category in _dataContext.Categories on m.CategoryId equals category.Id
                          join college in _dataContext.Colleges on m.CollegeId equals college.Id
                          join course in _dataContext.Courses on m.CourseId equals course.Id
                          join year in _dataContext.Years on m.YearId equals year.Id
                          where m.Id == id
                          select new CurrentSessionViewModel()
                          {
                              IdNo = m.IdNo,
                              FirstName = m.FirstName,
                              MiddleName = m.MiddleName,
                              LastName = m.LastName,
                              Age = m.Age,
                              Category = category.Name,
                              College = college.Name,
                              Course = course.Name,
                              Year = year.Name,
                              Section = m.Section,
                              Picture = m.Picture,
                              CurrentTime = DateTime.Now,
                              Notes = m.Notes,
                              IsFreeTier = category.IsFreeTier,
                              IsWithArrears = false,
                              Rate = 0,
                              Amount = 0
                          };

        var member = await memberQuery.SingleOrDefaultAsync();

        
        var rate = await GetRate();

        var session = await _dataContext.MemberSessions
            .Where(m => m.MemberId == id && m.EndTime == null).OrderByDescending(m => m.StartTime).FirstOrDefaultAsync();

        if (session != null && member != null)
        {
            var currentTime = (int)DateTime.Now.Subtract(session.StartTime).TotalMinutes;

            var credits = await _dataContext.MemberCredits
                .Where(m => m.MemberId == id).SumAsync(m => m.Credit);

            var usedSessions = await _dataContext.MemberSessions
                .Where(m => m.MemberId == id && m.EndTime.HasValue)
                .ToListAsync();

            var paidMinutes = await _dataContext.Payments
                .Where(m => m.MemberId == id)
                .SumAsync(m => m.Minutes);

            var usedTime = usedSessions.Select(m => (int)m.EndTime.Value.Subtract(m.StartTime).TotalMinutes).Sum();

            var remainingTime = 0;

            //decimal remainingTime = 0;
            decimal arrearAmount = 0;
            decimal minsArrears = 0;
            decimal hoursArrears = 0;
            decimal excessMinutes = 0;

            if (!member.IsFreeTier)
            {
                remainingTime = credits + paidMinutes - usedTime - currentTime;
            }

            decimal billableTime = 0;

            if (remainingTime < 0)
            {
                billableTime = -remainingTime;
            }
            var isWithArrears = await _dataContext.IBSTranHistories
                .Where(m => m.MemberId == id && m.ExcessMinutes > 0)
                .FirstOrDefaultAsync();
            if (isWithArrears != null)
            {
                member.IsWithArrears = true;
                excessMinutes = isWithArrears.ExcessMinutes;
                member.Rate = isWithArrears.Rate;
                minsArrears = excessMinutes % 60;
                hoursArrears = Math.Round((isWithArrears.ExcessMinutes - minsArrears) / 60);
                arrearAmount = (hoursArrears * member.Rate);
                member.Amount = arrearAmount;
            }

            return new CurrentSessionViewModel()
            {
                IdNo = member.IdNo,
                FirstName = member.FirstName,
                MiddleName = member.MiddleName,
                LastName = member.LastName,
                Age = member.Age,
                Category = member.Category,
                College = member.College,
                Course = member.Course,
                Year = member.Year,
                Section = member.Section,
                Picture = member.Picture,
                CurrentTime = DateTime.Now,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                TotalMinutes = currentTime,
                TimeAllotted = credits,
                RemainingTime = remainingTime,
                BillableAmount = billableTime * rate,
                Notes = member.Notes,
                IsFreeTier = member.IsFreeTier,
                IsWithArrears = member.IsWithArrears,
                Rate = member.Rate,
                Amount = member.Amount
            };
        }

        return null;
    }

    public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(int id)
    {
        var sessions = await _dataContext.MemberSessions.Where(m => m.MemberId == id)
            .OrderByDescending(m => m.StartTime)
            .ToListAsync();

        return sessions.Select(s => new SessionViewModel()
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            TotalMinutes = s.EndTime.HasValue ? (int)(s.EndTime - s.StartTime).Value.TotalMinutes : 0
        });
    }

    private async Task<decimal> GetRate()
    {
        return await _dataContext.Settings.Where(s => s.Name == "Rate").Select(s => decimal.Parse(s.Value)).SingleAsync();
    }


}