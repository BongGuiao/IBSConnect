using IBSConnect.Data.Models;

namespace IBSConnect.Business.Models;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Token = token;
    }

    public AuthenticateResponse(Member member, string token)
    {
        Id = member.Id;
        Token = token;
    }

}