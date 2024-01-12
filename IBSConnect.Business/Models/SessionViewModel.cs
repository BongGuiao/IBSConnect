using System;

namespace IBSConnect.Business.Models;

public class SessionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int TotalMinutes { get; set; }
}