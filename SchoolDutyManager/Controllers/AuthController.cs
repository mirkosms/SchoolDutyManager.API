using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using Microsoft.Extensions.Configuration;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(UserRegistrationDto loginDto)
        {
            var result = AuthService.Authenticate(loginDto.Email, loginDto.Password, _configuration["Jwt:Key"]);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }
            return Ok(result.Token);
        }
    }
}
