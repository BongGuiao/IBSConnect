using System;

namespace IBSConnect.Business;

public class UpdateBillingRequest
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}