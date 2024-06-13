using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Services;
using SchoolDutyManager.Models;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(UserRegistrationDto registrationDto)
        {
            var result = AuthService.RegisterUser(registrationDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("assign-role")]
        public IActionResult AssignRole(RoleAssignmentDto roleAssignmentDto)
        {
            var result = AuthService.AssignRole(roleAssignmentDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
