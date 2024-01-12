using System;

namespace IBSConnect.Business.Models;

public class CreateBillingRequest
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}