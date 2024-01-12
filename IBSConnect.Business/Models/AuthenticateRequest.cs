using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IBSConnect.Business.Models;

public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}



public class MemberAuthenticateRequest
{
    [Required]
    public string IdNo { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public IEnumerable<int> Applications { get; set; }
    [Required]
    public int UnitArea { get; set; }
}