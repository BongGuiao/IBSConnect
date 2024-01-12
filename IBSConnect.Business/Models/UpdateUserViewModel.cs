namespace IBSConnect.Business.Models;

public class UpdateUserViewModel
{
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}