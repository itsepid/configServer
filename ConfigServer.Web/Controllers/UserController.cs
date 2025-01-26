using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConfigServer.Domain.Interfaces;
using ConfigServer.Application.DTOs;
using ConfigServer.Infrastructure.Services;

namespace ConfigServer.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignupDTO signupDto)
        {
            var token = await _authService.SignupAsync(signupDto.Username, signupDto.Password);
            return Ok(new AuthResponseDto { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto.username, loginDto.Password);
             Response.Cookies.Append("AuthToken", token, new CookieOptions
    {
        HttpOnly = true,       
        Secure = true,            
        SameSite = SameSiteMode.Strict, 
        Expires = DateTime.UtcNow.AddMinutes(30) 
    });

    return Ok(new { message = "Login successful" });
        
        }


    }
}
