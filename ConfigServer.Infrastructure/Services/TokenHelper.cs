using Microsoft.AspNetCore.Http;
using System;
using ConfigServer.Domain.Interfaces;
namespace ConfigServer.Infrastructure.Services
{
public class TokenHelper
{
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenHelper(IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
    {
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public (int UserId, string Role) GetUserFromCookie()
    {
        
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is not available.");
        }

        
        if (!httpContext.Request.Cookies.TryGetValue("AuthToken", out var token))
        {
            throw new UnauthorizedAccessException("Token not found in cookies.");
        }

        
        return _jwtService.GetCurrentUser(token);
    }
}
}