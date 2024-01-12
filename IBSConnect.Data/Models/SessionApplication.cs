namespace IBSConnect.Data.Models;

public class SessionApplication
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int ApplicationId { get; set; }
    public MemberSession Session { get; set; }
    public Application Application { get; set; }
}