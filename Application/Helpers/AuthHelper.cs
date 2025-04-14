using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Application.Helpers;

public static class AuthHelper
{
    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return null;
        }
        var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
        return userName;
    }

    public static bool IsUserRole(ClaimsPrincipal principal)
    {
        return principal?.IsInRole(Role.User.ToString()) ?? false;
    }

    public static long? GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            return null;
        }
        
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(userIdClaim , out var userId) ? userId : 0;
    }
    
    public static JwtSecurityToken DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Invalid JWT token");
        }

        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken;
    }

    public static CallbackResponseModel JwtToModel(string token)
    {
        var jwtSecurityToken = DecodeToken(token);
        
        if (!DateTime.TryParse(jwtSecurityToken.Claims.FirstOrDefault(c => c.Type is ClaimTypes.Expiration)?.Value,
                out var dateTime))
        {
            dateTime = DateTime.Now.AddDays(1);
        };

        var claims = JwtClaimsToDictionary(jwtSecurityToken.Claims, token);

        return new CallbackResponseModel()
        {
            Principal = new CallbackPrincipalResponseModel()
            {
                Identity = new CallbackIdentityResponseModel()
                {
                    Claims = claims,
                    AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme
                }
            },
            Properties = new CallbackPropertiesResponseModel
            {
                IsPersistent = true,
                AllowRefresh = true,
                IssuedUtc = DateTime.SpecifyKind(jwtSecurityToken.IssuedAt, DateTimeKind.Utc),
                ExpiresUtc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) 
            }
        };
    }
    
    public static DateTime? UnixMillisecondsToDateTimeUtc(string? unixMilliseconds)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        if (long.TryParse(unixMilliseconds, out var expiration))
        {
            return dateTime.AddMilliseconds(expiration);
        }
        else return null;
    }

    private static Dictionary<string, string> JwtClaimsToDictionary(IEnumerable<Claim> claims, string token)
    {
        return new Dictionary<string, string>()
        {
            {
                ClaimTypes.NameIdentifier,
                claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? ""
            },
            {
                ClaimTypes.Name,
                claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? ""
            },
            {
                ClaimTypes.Email,
                claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? ""
            },
            {
                ClaimTypes.Authentication,
                token
            },
            {
                ClaimTypes.Expiration,
                claims.FirstOrDefault(c => c.Type == ClaimTypes.Expiration)?.Value ?? ""
            },
        };
    }
}