using System;

namespace IBSConnect.Business.Models;

public class PaymentViewModel
{
    public int Id { get; set; }
    public int Minutes { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
}