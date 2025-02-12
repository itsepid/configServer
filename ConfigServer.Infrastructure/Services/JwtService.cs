using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ConfigServer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using ConfigServer.Application.Interfaces;

namespace ConfigServer.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Secret"];
        }

        public string GenerateJwt(User user)
        {
           
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username), 
                new Claim("userId", user.Id.ToString()),   
                new Claim(ClaimTypes.Role, user.Roles.ToString())   
            };

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: "configserver",
                audience: "configserverAPI",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

           
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateJwt(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = "configserver",
                    ValidAudience = "configserverAPI",
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return claimsPrincipal;
            }
            catch
            {
                
                return null;
            }
        }

        public (int UserId, string Role) GetCurrentUser(string token)
        {
            
            var claimsPrincipal = ValidateJwt(token);
            if (claimsPrincipal == null)
                throw new Exception("Invalid token.");

            
            foreach (var claim in claimsPrincipal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            
            var userIdClaim = claimsPrincipal.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("UserId claim not found in token.");

            
            var userRoleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userRoleClaim))
                throw new Exception("Role claim not found in token.");

            
            return (int.Parse(userIdClaim), userRoleClaim);
        }
    }
}