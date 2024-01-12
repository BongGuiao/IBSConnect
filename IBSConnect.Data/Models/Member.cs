using System;
using System.Collections.Generic;

namespace IBSConnect.Data.Models;

public class Member
{
    public Member()
    {
        Credits = new List<MemberCredit>();
        Sessions = new List<MemberSession>();
    }

    public int Id { get; set; }
    public string IdNo { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Picture { get; set; }
    public string MimeType { get; set; }
    public int CategoryId { get; set; }
    public int YearId { get; set; }
    public int CourseId { get; set; }
    public string Section { get; set; }
    public int CollegeId { get; set; }
    public string Remarks { get; set; }
    public bool IsActive { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Notes { get; set; }
    public ICollection<MemberCredit> Credits { get; set; }
    public ICollection<MemberSession> Sessions { get; set; }
    public ICollection<Payment> Payments { get; set; }
}