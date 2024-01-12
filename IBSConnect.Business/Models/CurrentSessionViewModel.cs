using System;

namespace IBSConnect.Business.Models;

public class CurrentSessionViewModel
{
    public string IdNo { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Picture { get; set; }
    public string MimeType { get; set; }
    public string Category { get; set; }
    public string Year { get; set; }
    public string Course { get; set; }
    public string Section { get; set; }
    public string College { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime CurrentTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int TotalMinutes { get; set; }
    public int RemainingTime { get; set; }
    public int TimeAllotted { get; set; }
    public decimal BillableAmount { get; set; }
    public string Notes { get; set; }
    public bool IsFreeTier { get; set; }
    public bool IsWithArrears { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
}