using System;
using System.Collections.Generic;

namespace IBSConnect.Data.Models;

public class MemberSession
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ICollection<SessionApplication> Applications { get; set; }
    public UnitArea UnitArea { get; set; }
}