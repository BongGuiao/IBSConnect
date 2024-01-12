using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using IBSConnect.Security;

namespace IBSConnect.AngularApp.Controllers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public List<string> Roles { get; }

    public AuthorizeAttribute()
    {
        Roles = new List<string>();
    }

    public AuthorizeAttribute(params string[] roles)
    {
        Roles = roles.Select(r => r.ToLower()).ToList();
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User is IBSPrincipal user)
        {
            if (!Roles.Contains(((IBSIdentity)user.Identity).Role.ToLower()))
            {
                throw new UnauthorizedAccessException();
            }
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
    }
}