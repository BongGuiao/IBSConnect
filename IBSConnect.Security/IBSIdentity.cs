using System.Security.Claims;

namespace IBSConnect.Security;

public class IBSIdentity : ClaimsIdentity
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
    public DateTime TokenExpires { get; }
    public override string AuthenticationType => "IBS Authentication";
    public override bool IsAuthenticated { get; }
    public override string Name { get; }

    private IBSIdentity()
    {
    }

    public IBSIdentity(int id, string userName, string fullname, string role, DateTime tokenExpires, IEnumerable<Claim> claims) : base(claims)
    {
        Id = id;
        Name = userName;
        IsAuthenticated = true;
        FullName = fullname;
        TokenExpires = tokenExpires;
        Role = role;
    }

    public static IBSIdentity Unauthenticated()
    {
        return new IBSIdentity();
    }

}