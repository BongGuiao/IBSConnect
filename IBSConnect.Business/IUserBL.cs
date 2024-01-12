using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface IUserBL
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
    Task<IEnumerable<UserListViewModel>> GetAllAsync();
    Task<UserListViewModel> GetByIdAsync(int id);
    Task<int> CreateAsync(UserViewModel user);
    Task UpdateAsync(int id, UpdateUserViewModel user, int userId);
    Task DeleteAsync(int id, int userId);
    Task<UserListViewModel> GetByUserNameAsync(string username);
}