namespace IBSConnect.Business.Models;

public class UserListViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public bool IsInUse { get; set; }
}