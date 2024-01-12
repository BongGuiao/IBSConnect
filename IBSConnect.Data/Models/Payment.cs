using System;

namespace IBSConnect.Data.Models;

public class Payment
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int Minutes { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
}