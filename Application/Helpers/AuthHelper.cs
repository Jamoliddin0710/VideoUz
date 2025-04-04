using System.Security.Claims;

namespace Application.Helpers;

public static class AuthHelper
{
    public static string? GetUserName(ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return null;
        }
        var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
        return userName;
    }

    public static long? GetUserId(ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return null;
        }
        
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim , out var userId) ? userId : 0;
    }
}