using System;

namespace IBSConnect.Business;

public class MemberBillViewModel
{
    public int MemberId { get; set; }
    public string IdNo { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public int AllottedTime { get; set; }
    public int? PaidMinutes { get; set; }
    public int? TotalMinutes { get; set; }
    public int? TimeLeft { get; set; }
    public int ExcessMinutes { get; set; }
    public decimal Rate { get; set; }
    public decimal Charge { get; set; }
    public DateTime Date { get; set; }
}