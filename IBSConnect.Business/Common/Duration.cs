using System;

namespace IBSConnect.Business.Common;

public class Duration
{
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
}

public static class DurationExtensions
{
    public static DateTime FromUtcNow(this Duration duration)
    {
        return DateTime.UtcNow
            .AddDays(duration.Days)
            .AddHours(duration.Hours)
            .AddMinutes(duration.Minutes)
            .AddSeconds(duration.Seconds);
    }
}