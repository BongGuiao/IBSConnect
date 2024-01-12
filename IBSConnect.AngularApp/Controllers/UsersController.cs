using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;

namespace IBSConnect.AngularApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : AuthorizedUserControllerBase
{
    private readonly IUserBL _userBl;

    public UsersController(IUserBL userBl)
    {
        _userBl = userBl;
    }

    [HttpPost("authenticate")]
    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        return await _userBl.AuthenticateAsync(model);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet("{id}")]
    public async Task<UserListViewModel> Get(int id)
    {
        return await _userBl.GetByIdAsync(id);
    }

    [Authorize(Roles.Administrator)]
    [HttpPost]
    public async Task Create(UserViewModel user)
    {
        await _userBl.CreateAsync(user);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("{id}")]
    public async Task Update(int id, [FromBody] UpdateUserViewModel user)
    {
        await _userBl.UpdateAsync(id, user, CurrentIdentity.Id);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _userBl.DeleteAsync(id, CurrentIdentity.Id);
    }

    [Authorize(Roles.Administrator)]
    [HttpGet]
    public async Task<IEnumerable<UserListViewModel>> GetAll()
    {
        return await _userBl.GetAllAsync();
    }
}