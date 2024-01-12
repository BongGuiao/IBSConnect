namespace IBSConnect.Business.Models;

public class IBSTranHistoryView
{
    public int MemberId { get; set; }
    public string IdNo { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public decimal AllottedTime { get; set; }
    public decimal TotalMinutes { get; set; }
    public decimal PaidMinutes { get; set; }
    public decimal TimeLeft { get; set; }
    public decimal ExcessMinutes { get; set; }
    public string SySemester { get; set; }
    public decimal Rate { get; set; }

}