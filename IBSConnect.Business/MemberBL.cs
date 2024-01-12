using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using IBSConnect.Data.Models;
using MySql.Data.MySqlClient;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using NCrypt = BCrypt.Net.BCrypt;
using MySqlX.XDevAPI;

namespace IBSConnect.Business;

public class MemberBL : IMemberBL
{
    private readonly IBSConnectDataContext _dataContext;
    private readonly IMapper _mapper;

    private readonly AppSettings _appSettings;

    public MemberBL(IBSConnectDataContext dataContext, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<ImportResult> ImportMembersAsync(IEnumerable<Member> members)
    {
        var result = new ImportResult();

        var idNos = members.Select(m => m.IdNo);

        var existingMembers = await _dataContext.Members.Where(m => idNos.Contains(m.IdNo)).ToListAsync();

        var existingMemberIds = existingMembers.Select(m => m.IdNo);

        var defaultTime = await GetDefaultTime();
        var password = await GetDefaultPassword();

        var lookup = members.ToDictionary(m => m.IdNo);

        // Existing Members
        foreach (var member in existingMembers)
        {
            if (lookup.TryGetValue(member.IdNo, out var user))
            {
                member.IdNo = user.IdNo;
                member.FirstName = user.FirstName.Trim();
                member.MiddleName = user.MiddleName.Trim();
                member.LastName = user.LastName.Trim();
                member.Age = user.Age;
                // TODO: Set ACTIVE/INACTIVE
                //u.IsActive = user.IsActive;
                member.Section = user.Section.Trim();
                member.CategoryId = user.CategoryId;
                member.CollegeId = user.CollegeId;
                member.CourseId = user.CourseId;
                member.YearId = user.YearId;
                member.Notes = user.Notes;
                result.Updated += 1;
            }
        }


        // New Members
        foreach (var member in members.Where(m => !existingMemberIds.Contains(m.IdNo)))
        {
            member.PasswordHash = NCrypt.HashPassword(password);
            member.IsActive = true;
            member.CreatedDate = DateTime.Now;
            member.Credits = new List<MemberCredit>()
            {
                new MemberCredit()
                {
                    Credit = defaultTime,
                    CreatedDate = DateTime.Now
                }
            };

            result.Added += 1;
            _dataContext.Members.Add(member);
        }

        await _dataContext.SaveChangesAsync();

        return result;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(MemberAuthenticateRequest model)
    {
        var member = await _dataContext.Members
            .SingleOrDefaultAsync(x => x.IdNo == model.IdNo && x.IsActive);

        if (member == null || !NCrypt.Verify(model.Password, member.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        var token = GenerateJwtToken(member);

        return new AuthenticateResponse(member, token);

    }

    public async Task<QueryResult<MemberListItemModel>> GetAllAsync(FilterRequest filter)
    {
        var query = from m in _dataContext.Members
                    join category in _dataContext.Categories on m.CategoryId equals category.Id
                    join college in _dataContext.Colleges on m.CollegeId equals college.Id
                    join course in _dataContext.Courses on m.CourseId equals course.Id
                    join year in _dataContext.Years on m.YearId equals year.Id
                    select new MemberListItemModel()
                    {
                        Id = m.Id,
                        IdNo = m.IdNo,
                        FirstName = m.FirstName,
                        MiddleName = m.MiddleName,
                        LastName = m.LastName,
                        Age = m.Age,
                        Picture = m.Picture,
                        Section = m.Section,
                        Category = category.Name,
                        College = college.Name,
                        Course = course.Name,
                        Year = year.Name,
                    };

        List<Expression<Func<MemberListItemModel, bool>>> queryFilters = new List<Expression<Func<MemberListItemModel, bool>>>();

        if (filter.Query != null && filter.Query.Length > 0)
        {
            var parts = filter.Query.Split(new[] { ',', ' ' });
            foreach (var part in parts)
            {
                queryFilters.Add(member => member.IdNo.Contains(part)
                                         || member.FirstName.StartsWith(part)
                                         || member.LastName.StartsWith(part));
            }
        };


        var (total, filteredQuery) = await query.PagedFilterAsync(filter, queryFilters.ToArray());


        var result = await filteredQuery.ToListAsync();

        return new QueryResult<MemberListItemModel>()
        {
            Count = total,
            Result = result
        };
    }

    public async Task<MemberViewModel> GetByIdAsync(int id)
    {
        var member = await (from m in _dataContext.Members
                            where m.Id == id
                            select new MemberViewModel()
                            {
                                Id = m.Id,
                                IdNo = m.IdNo,
                                FirstName = m.FirstName,
                                MiddleName = m.MiddleName,
                                LastName = m.LastName,
                                Age = m.Age,
                                Picture = m.Picture,
                                Section = m.Section,
                                CategoryId = m.CategoryId,
                                CollegeId = m.CollegeId,
                                CourseId = m.CourseId,
                                YearId = m.YearId,
                                Notes = m.Notes,
                            }
            ).SingleAsync();

        var category = await _dataContext.Categories.Where(m => m.Id == member.CategoryId).FirstAsync();

        var credits = await _dataContext.MemberCredits.Where(m => m.MemberId == id).SumAsync(m => m.Credit);

        var paidMinutes = await _dataContext.Payments.Where(m => m.MemberId == id).SumAsync(m => m.Minutes);

        var sessions = await _dataContext.MemberSessions.Where(m => m.MemberId == id)
            .ToListAsync();

        var totalMinutes = sessions.Select(s => s.EndTime.HasValue ? (int)(s.EndTime - s.StartTime).Value.TotalMinutes : 0).Sum();

        member.Credits = credits;
        member.TotalMinutes = totalMinutes;

        if (!category.IsFreeTier)
        {
            member.RemainingMinutes = credits + paidMinutes - totalMinutes;
        }

        return member;
    }

    public async Task CreditMinutesAsync(int id, int minutes)
    {
        _dataContext.MemberCredits.Add(new MemberCredit()
        {
            MemberId = id,
            CreatedDate = DateTime.Now,
            Credit = minutes
        });

        await _dataContext.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(CreateMemberRequest user)
    {
        try
        {
            //var validator = new UserValidator(true);
            //var validationResult = validator.Validate(user);

            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
            //}

            var defaultTime = await GetDefaultTime();

            var dbuser = new Member
            {
                IdNo = user.IdNo.Trim(),
                FirstName = user.FirstName.Trim(),
                MiddleName = user.MiddleName.Trim(),
                LastName = user.LastName.Trim(),
                PasswordHash = NCrypt.HashPassword(user.Password),
                //Role = user.Role,
                Age = user.Age,
                Section = user.Section.Trim(),
                CategoryId = user.CategoryId,
                CollegeId = user.CollegeId,
                CourseId = user.CourseId,
                YearId = user.YearId,
                IsActive = true,
                CreatedDate = DateTime.Now,
                Credits = new List<MemberCredit>()
                {
                    new MemberCredit()
                    {
                       Credit = defaultTime,
                       CreatedDate = DateTime.Now
                    }
                }
            };

            if (user.Picture != null)
            {
                var (filename, mimetype) = SaveDataUrlToFile(user.Picture, _appSettings.ImagePath, Guid.NewGuid().ToString());

                dbuser.Picture = filename;
                dbuser.MimeType = mimetype;
            }

            await _dataContext.Members.AddAsync(dbuser);

            await _dataContext.SaveChangesAsync();

            return dbuser.Id;
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException.Message.StartsWith("Duplicate"))
            {
                throw new IBSConnectException("A user exists with the same username");
            }
            throw;
        }
    }


    private (string, string) SaveDataUrlToFile(string dataUrl, string savePath, string filename)
    {
        var matchGroups = Regex.Match(dataUrl, @"^data:((?<type>[\w\/]+))?;base64,(?<data>.+)$").Groups;
        var base64Data = matchGroups["data"].Value;
        var mimetype = matchGroups["type"].Value;
        var extension = MimeTypes.MimeTypeMap.GetExtension(mimetype);
        filename += extension;
        var binData = Convert.FromBase64String(base64Data);
        File.WriteAllBytes(Path.Combine(savePath, filename), binData);
        return (filename, mimetype);
    }

    //public async Task<(Stream, string)> GetImageAsync(int id)
    //{
    //    var image = await _dataContext.Members.Where(m => m.Id == id).Select(m => new { m.Picture, m.MimeType }).FirstOrDefaultAsync();

    //    var path = Path.Combine(_appSettings.ImagePath, image.Picture);

    //    return (File.Open(path, FileMode.Open, FileAccess.Read), image.MimeType);
    //}


    public async Task<(Stream, string)> GetImageAsync(string filename)
    {
        var path = Path.Combine(_appSettings.ImagePath, filename);

        return (File.Open(path, FileMode.Open, FileAccess.Read), "image/jpg");
    }

    public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(int id)
    {
        var sessions = await _dataContext.MemberSessions.Where(m => m.MemberId == id)
            .OrderByDescending(m => m.StartTime)
            .Select(m => new SessionViewModel()
            {
                Id = m.Id,
                StartTime = m.StartTime,
                EndTime = m.EndTime,
            }).ToListAsync();

        foreach (var session in sessions)
        {
            session.TotalMinutes = session.EndTime.HasValue
                ? (int)(session.EndTime - session.StartTime).Value.TotalMinutes
                : (int)(DateTime.Now - session.StartTime).TotalMinutes;
        }

        return sessions;
    }

    private IEnumerable<MemberSession> FilterSessions(IEnumerable<MemberSession> sessions)
    {
        var cutoff = _appSettings.CutoffHours * 60;

        foreach (var session in sessions)
        {
            var totalMinutes = session.EndTime.HasValue
                ? (int)(session.EndTime - session.StartTime).Value.TotalMinutes
                : (int)(DateTime.Now - session.StartTime).TotalMinutes;

            if (totalMinutes > cutoff)
            {
                yield return session;
            }
        }
    }

    public async Task CloseActiveSessionsAsync()
    {
        var query = from ms in _dataContext.MemberSessions
                    join m in _dataContext.Members on ms.MemberId equals m.Id
                    where ms.EndTime == null
                    orderby ms.StartTime descending
                    select ms;

        var sessions = await query.ToListAsync();

        var cutoffSessions = FilterSessions(sessions);

        foreach (var session in cutoffSessions)
        {
            session.EndTime = DateTime.Now;
        }

        await _dataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SessionViewModel>> GetActiveSessionsAsync()
    {
        var query = from ms in _dataContext.MemberSessions
                    join m in _dataContext.Members on ms.MemberId equals m.Id
                    where ms.EndTime == null
                    orderby ms.StartTime descending
                    select new SessionViewModel()
                    {
                        Name = $"{m.FirstName} {m.LastName}",
                        StartTime = ms.StartTime,
                    };

        var sessions = await query.ToListAsync();

        foreach (var session in sessions)
        {
            session.TotalMinutes = session.EndTime.HasValue
                ? (int)(session.EndTime - session.StartTime).Value.TotalMinutes
                : (int)(DateTime.Now - session.StartTime).TotalMinutes;
        }

        var totalMinutes = _appSettings.CutoffHours * 60;

        return sessions.Where(t => t.TotalMinutes > totalMinutes);
    }

    public async Task UpdateSessionAsync(int id, SessionViewModel session)
    {
        var _session = await _dataContext.MemberSessions.Where(m => m.MemberId == id && m.Id == session.Id).FirstOrDefaultAsync();
        if (session.EndTime <= session.StartTime)
        {
            throw new ValidationException(new[] { "End time must be after start time." });
        }
        _session.StartTime = session.StartTime;
        _session.EndTime = session.EndTime;

        await _dataContext.SaveChangesAsync();
    }

    public async Task<MemberBillViewModel> GetBillAsync(int id)
    {
        var rate = await GetRate();

        var bill = await _dataContext.MemberBills
            .Where(m => m.MemberId == id).Select(m =>
                new MemberBillViewModel()
                {
                    MemberId = m.MemberId,
                    IdNo = m.IdNo,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    AllottedTime = m.AllottedTime,
                    PaidMinutes = m.PaidMinutes,
                    TotalMinutes = m.TotalMinutes,
                    TimeLeft = m.TimeLeft,
                    ExcessMinutes = m.ExcessMinutes
                }).SingleAsync();

        bill.Date = DateTime.Now;
        bill.Rate = rate;
        bill.Charge = bill.ExcessMinutes * rate;
        return bill;
    }

    public async Task<QueryResult<MemberBillViewModel>> GetBillingAsync(BillingFilterRequest filter)
    {
        var rate = await GetRate();

        var query = from m in _dataContext.MemberBills
                    select new MemberBillViewModel()
                    {
                        MemberId = m.MemberId,
                        IdNo = m.IdNo,
                        FirstName = m.FirstName,
                        MiddleName = m.MiddleName,
                        LastName = m.LastName,
                        PaidMinutes = m.PaidMinutes,
                        AllottedTime = m.AllottedTime,
                        TotalMinutes = m.TotalMinutes,
                        TimeLeft = m.TimeLeft,
                        ExcessMinutes = m.ExcessMinutes,
                    };

        Expression<Func<MemberBillViewModel, bool>> queryFilter = null;
        Expression<Func<MemberBillViewModel, bool>> showAllFilter = null;

        if (filter.Query != null && filter.Query.Length > 0)
        {
            queryFilter = member => member.IdNo.Contains(filter.Query.Trim())
                                    || member.FirstName.StartsWith(filter.Query.Trim())
                                    || member.LastName.StartsWith(filter.Query.Trim());
        };


        if (!filter.ShowAll)
        {
            showAllFilter = member => member.ExcessMinutes > 0;
        };

        var (total, filteredQuery) = await query.PagedFilterAsync(filter, queryFilter, showAllFilter);

        var result = await filteredQuery.ToListAsync();

        foreach (var bill in result)
        {
            bill.Rate = rate;
            bill.Charge = bill.ExcessMinutes * rate;
        }

        return new QueryResult<MemberBillViewModel>()
        {
            Count = total,
            Result = result
        };
    }

    public async Task UpdateAsync(int id, UpdateMemberRequest user)
    {
        var context = (DbContext)_dataContext;
        var u = context.Find<Member>(id);

        u.IdNo = user.IdNo;
        u.FirstName = user.FirstName.Trim();
        u.MiddleName = user.MiddleName.Trim();
        u.LastName = user.LastName.Trim();
        u.Age = user.Age;
        // TODO: Set ACTIVE/INACTIVE
        //u.IsActive = user.IsActive;

        u.Section = user.Section.Trim();
        u.CategoryId = user.CategoryId;
        u.CollegeId = user.CollegeId;
        u.CourseId = user.CourseId;
        u.YearId = user.YearId;
        u.Notes = user.Notes;

        if (!string.IsNullOrEmpty(user.Password))
        {
            if (user.Password.Length < 8)
            {
                throw new ValidationException(new[] { "The password must be at least 8 characters in length" });
            }

            u.PasswordHash = NCrypt.HashPassword(user.Password);
        }

        if (user.Picture != null)
        {
            var (filename, mimetype) = SaveDataUrlToFile(user.Picture, _appSettings.ImagePath, Guid.NewGuid().ToString());

            u.Picture = filename;
            u.MimeType = mimetype;
        }

        context.Update(u);

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var connectionString = _dataContext.Database.GetConnectionString();

        var sql = @"
DELETE sa FROM session_applications sa
INNER JOIN member_sessions ms ON sa.SessionId = ms.Id
WHERE MemberId = @Id;
DELETE FROM member_sessions WHERE MemberId = @Id;
DELETE FROM member_credits WHERE MemberId = @Id;
DELETE FROM members WHERE Id = @Id;
";

        await using var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        //var command = connection.CreateCommand();

        //command.CommandText = sql;
        //command.Parameters.Add("Id", MySqlDbType.Int32).Value = id;
        //await command.ExecuteNonQueryAsync();


        await using var tran = await connection.BeginTransactionAsync();

        try
        {
            await connection.ExecuteAsync(sql, new { Id = id });

            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }

    }

    public async Task<IEnumerable<PaymentViewModel>> GetPaymentsAsync(int id)
    {
        return await _dataContext.Payments.OrderByDescending(m => m.CreatedDate).Where(m => m.MemberId == id)
            .Select(m => new PaymentViewModel()
            {
                Id = m.Id,
                Minutes = m.Minutes,
                Rate = m.Rate,
                Amount = m.Amount,
                CreatedDate = m.CreatedDate
            }).ToListAsync();
    }

    public async Task<IEnumerable<IBSTranHistoryView>> GetTotalArrearsAsync(int id)
    {
        return await _dataContext.IBSTranHistories.OrderByDescending(m => m.SySemester).Where(m => m.MemberId == id && m.ExcessMinutes > 0)
            .Select(m => new IBSTranHistoryView()
            {
                MemberId = m.MemberId,
                IdNo = m.IdNo,
                SySemester = m.SySemester,
                AllottedTime = m.AllottedTime,
                TotalMinutes = m.TotalMinutes,
                PaidMinutes = m.PaidMinutes,
                TimeLeft = m.TimeLeft,
                ExcessMinutes = m.ExcessMinutes,
                FirstName = m.FirstName,
                MiddleName = m.MiddleName,
                LastName = m.LastName,
                Rate = m.Rate
            }).ToListAsync();
    }

    public async Task AddPaymentAsync(int id, decimal amount, int userId)
    {
        var rate = await GetRate();

        var minutes = amount / rate;

        _dataContext.Payments.Add(new Payment()
        {
            MemberId = id,
            Minutes = (int)minutes,
            Rate = rate,
            Amount = amount,
            UserId = userId,
            CreatedDate = DateTime.Now,
        });

        await _dataContext.SaveChangesAsync();
    }

    public async Task AddPaymentArrearsAsync(int id, decimal amount, int userId)
    {
        var rate = await GetRate();

        var minutes = amount / rate;
        var arrearsHistory = await _dataContext.IBSTranHistories.OrderByDescending(x => x.SySemester)
            .Where(x => x.MemberId == id && x.ExcessMinutes > 0)
            .ToListAsync();
        if (arrearsHistory.Count() > 0)
        {
            arrearsHistory[0].PaidMinutes = arrearsHistory[0].ExcessMinutes;
            arrearsHistory[0].ExcessMinutes = 0;
            _dataContext.IBSTranHistories.Update(arrearsHistory[0]);
            rate = arrearsHistory[0].Rate;
        }

        _dataContext.PaymentArrears.Add(new PaymentArrear()
        {
            MemberId = id,
            Minutes = (int)minutes,
            Rate = rate,
            Amount = amount,
            UserId = userId,
            CreatedDate = DateTime.Now,
        });



        await _dataContext.SaveChangesAsync();
    }

    // helper methods

    private string GenerateJwtToken(Member user)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // TODO: add new claims here
                new Claim("id", user.Id.ToString()),
                new Claim("role", Roles.Member),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            }),
            Expires = _appSettings.WebTokenExpiry.FromUtcNow(),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private async Task<int> GetDefaultTime()
    {
        return await _dataContext.Settings.Where(s => s.Name == "DefaultTime").Select(s => int.Parse(s.Value)).SingleAsync();
    }

    private async Task<decimal> GetRate()
    {
        return await _dataContext.Settings.Where(s => s.Name == "Rate").Select(s => decimal.Parse(s.Value)).SingleAsync();
    }

    private async Task<string> GetDefaultPassword()
    {
        return await _dataContext.Settings.Where(s => s.Name == "DefaultPassword").Select(s => s.Value).SingleAsync();
    }

    public async Task<IEnumerable<PaymentArrearsViewModel>> GetPaymentArrearsAsync(int id)
    {
        return await _dataContext.PaymentArrears.OrderByDescending(m => m.CreatedDate).Where(m => m.MemberId == id)
            .Select(m => new PaymentArrearsViewModel()
            {
                Id = m.Id,
                Minutes = m.Minutes,
                Rate = m.Rate,
                Amount = m.Amount,
                CreatedDate = m.CreatedDate
            }).ToListAsync();
    }


}