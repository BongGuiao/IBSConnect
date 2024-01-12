namespace IBSConnect.Business.Models;

public class MemberViewModel
{
    public int Id { get; set; }
    public string IdNo { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int Age { get; set; }
    public string Picture { get; set; }
    public int CategoryId { get; set; }
    public int YearId { get; set; }
    public int CourseId { get; set; }
    public string Section { get; set; }
    public int CollegeId { get; set; }
    public int Credits { get; set; }
    public int TotalMinutes { get; set; }
    public int RemainingMinutes { get; set; }
    public string Notes { get; set; }
}