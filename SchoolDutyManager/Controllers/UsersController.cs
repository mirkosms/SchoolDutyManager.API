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
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("current")]
        [Authorize]
        public ActionResult<User> GetCurrentUser()
        {
            var email = User.Identity.Name;
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Student, Teacher, Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet]
        [Route("role/{role}")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        public ActionResult<List<User>> GetUsersByRole(string role)
        {
            var users = userService.GetUsersByRole(role);
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            var user = userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            userService.DeleteUser(id);
            return NoContent();
        }
    }
}
