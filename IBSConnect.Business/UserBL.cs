using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using IBSConnect.Business.Validators;
using IBSConnect.Data;
using IBSConnect.Data.Models;
using NCrypt = BCrypt.Net.BCrypt;

namespace IBSConnect.Business;

public class UserBL : IUserBL
{
    private readonly IBSConnectDataContext _dataContext;
    private readonly IMapper _mapper;

    private readonly AppSettings _appSettings;

    public UserBL(IBSConnectDataContext dataContext, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
    { var user = await _dataContext.Users
            .SingleOrDefaultAsync(x => x.Username == model.Username && x.IsActive);

        if (user == null || !NCrypt.Verify(model.Password, user.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);

    }

    public async Task<IEnumerable<UserListViewModel>> GetAllAsync()
    {
        return await _dataContext.Users
            .Select(u =>
                new UserListViewModel()
                {
                    UserName = u.Username,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Id = u.Id,
                    IsActive = u.IsActive,
                    //Role = u.Role
                })
            .ToListAsync();
    }

    public async Task<UserListViewModel> GetByIdAsync(int id)
    {
        return await _dataContext.Users.Where(u => u.Id == id)
            .Select(u => _mapper.Map<UserListViewModel>(u))
            .FirstOrDefaultAsync();
    }

    public async Task<UserListViewModel> GetByUserNameAsync(string username)
    {
        return await _dataContext.Users.Where(x => x.Username == username)
            .Select(u => _mapper.Map<UserListViewModel>(u))
            .FirstOrDefaultAsync();
    }


    public async Task<int> CreateAsync(UserViewModel user)
    {
        try
        {
            var validator = new UserValidator(true);
            var validationResult = validator.Validate(user);
                
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var dbuser = new User
            {
                Username = user.Username,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                PasswordHash = NCrypt.HashPassword(user.Password),
                //Role = user.Role,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            await _dataContext.Users.AddAsync(dbuser);
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

    public async Task UpdateAsync(int id, UpdateUserViewModel user, int userId)
    {
        var salt = Cryptography.Salt();
        var context = (DbContext)_dataContext;
        var u = context.Find<User>(id);

        u.FirstName = user.FirstName;
        u.LastName = user.LastName;

        //if (userId == u.Id && u.Role != user.Role)
        //{
        //    throw new IBSConnectException("You cannot change your own role as this may result in lock-out");
        //}

        //if (userId == u.Id && !user.IsActive)
        //{
        //    throw new IBSConnectException("You cannot change your own status as this may result in lock-out");
        //}

        //u.IsActive = user.IsActive;
        //u.Role = user.Role;

        if (!string.IsNullOrEmpty(user.Password))
        {
            if (user.Password.Length < 8)
            {
                throw new ValidationException(new[] { "The password must be at least 8 characters in length" });
            }

            u.PasswordHash = NCrypt.HashPassword(user.Password);
        }

        context.Update(u);

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        if (id == userId)
        {
            throw new IBSConnectException("You cannot remove youself as this may result in lock-out");
        }

        var context = (DbContext)_dataContext;
        var u = context.Find<User>(id);
        context.Remove(u);
        await _dataContext.SaveChangesAsync();
    }

    // helper methods

    private string GenerateJwtToken(User user)
    {
        // generate token that is valid for 4 hours
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // TODO: add new claims here
                new Claim("id", user.Id.ToString()),
                new Claim("role", Roles.Administrator),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("userName", user.Username),
            }),
            Expires = _appSettings.AdminTokenExpiry.FromUtcNow(),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}