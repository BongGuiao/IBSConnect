using System.Security.Claims;
using System.Security.Principal;

namespace IBSConnect.Security;

public class IBSPrincipal : ClaimsPrincipal
{
    public override IIdentity Identity { get; }

    public override bool IsInRole(string role)
    {
        return ((IBSIdentity)Identity).Role == role;
    }

    public IBSPrincipal(IBSIdentity identity)
    {
        Identity = identity;
    }

    public static IBSPrincipal Unauthenticated()
    {
        return new IBSPrincipal(IBSIdentity.Unauthenticated());
    }
}