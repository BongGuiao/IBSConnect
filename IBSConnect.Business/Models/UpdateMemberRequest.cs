namespace IBSConnect.Business.Models;

public class UpdateMemberRequest
{
    public string IdNo { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int Age { get; set; }
    public string Picture { get; set; }
    public string Section { get; set; }
    public int CategoryId { get; set; }
    public int CollegeId { get; set; }
    public int CourseId { get; set; }
    public int YearId { get; set; }
    public bool IsActive { get; set; }
    public string Notes { get; set; }
}
