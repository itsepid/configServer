using System.Security.Claims;
using ConfigServer.Domain.Entities;

namespace ConfigServer.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwt(User user);
        ClaimsPrincipal ValidateJwt(string token);
        (int UserId, string Role) GetCurrentUser(string token);
    }
}
