using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var token = AuthService.Login(loginDto);
            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto registrationDto)
        {
            var result = AuthService.RegisterUser(registrationDto);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(new { Message = result.Message });
        }

        [HttpPost("assign-role")]
        public IActionResult AssignRole([FromBody] RoleAssignmentDto roleAssignmentDto)
        {
            var result = AuthService.AssignRole(roleAssignmentDto);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(new { Message = result.Message });
        }

        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var email = User.Identity.Name;
            var user = AuthService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var userInfo = new
            {
                user.Email,
                Roles = user.Roles
            };

            return Ok(userInfo);
        }
    }
}
