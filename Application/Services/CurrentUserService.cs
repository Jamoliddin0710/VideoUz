using System.Security.Claims;
using Application.ServiceContract;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public string GetUserId()
    {
        return _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
    
    public string GetIpAddress()
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        
        string ip = httpContext?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(ip))
        {
            ip = httpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    
        return ip ?? "Unknown";
    }
}