using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolDutyManager.Models;
using SchoolDutyManager.Services;
using System.Collections.Generic;

namespace SchoolDutyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Route("current")]
        [Authorize]
        public ActionResult<User> GetCurrentUser()
        {
            var email = User.Identity.Name;
            var user = AuthService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = AuthService.GetAllUsers();
            return Ok(users);
        }
    }
}
