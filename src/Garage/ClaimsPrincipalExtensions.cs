using System.Security.Claims;
using Garage.Constants;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace Garage;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated??false)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Defaults.Users.UserId;
        }
        return Defaults.Users.UserId;
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated ?? false)
        {
            return principal.Identity?.Name ?? Defaults.Users.UserName;
        }
        return Defaults.Users.UserName;
    }
}