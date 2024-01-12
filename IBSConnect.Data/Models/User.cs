using System;

namespace IBSConnect.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }
}