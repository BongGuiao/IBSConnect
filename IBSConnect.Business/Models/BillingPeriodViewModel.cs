using System;

namespace IBSConnect.Business.Models;

public class BillingPeriodViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}