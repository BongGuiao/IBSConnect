using Microsoft.AspNetCore.Mvc;
using IBSConnect.Security;

namespace IBSConnect.AngularApp.Controllers;

public class AuthorizedUserControllerBase : ControllerBase
{
    protected IBSPrincipal CurrentPrincipal => (IBSPrincipal)HttpContext.User;

    protected IBSIdentity CurrentIdentity => (IBSIdentity)((IBSPrincipal)HttpContext.User).Identity;

}