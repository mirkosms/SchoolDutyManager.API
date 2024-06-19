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
        [Authorize(Roles = "Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = userService.GetAllUsers();
            return Ok(users);
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

        [HttpPut("current")]
        [Authorize]
        public IActionResult UpdateCurrentUser([FromBody] UserUpdateDto updateUserDto)
        {
            var email = User.Identity.Name;
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Password = updateUserDto.Password; // Aktualizacja hasła
            userService.UpdateUser(user);

            return Ok(new { Success = true, Message = "Profile updated successfully" });
        }
    }
}
