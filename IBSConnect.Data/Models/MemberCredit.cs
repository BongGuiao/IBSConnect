using System;

namespace IBSConnect.Data.Models;

public class MemberCredit
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int Credit { get; set; }
    public DateTime CreatedDate { get; set; }
}