using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolDutyManager.Services
{
    public static class AuthService
    {
        private static List<User> users = new List<User>();

        public static string Login(UserLoginDto loginDto)
        {
            var user = users.FirstOrDefault(u => u.Email == loginDto.Email && u.Password == loginDto.Password);
            if (user == null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        public static AuthResult RegisterUser(UserRegistrationDto registrationDto)
        {
            if (users.Any(u => u.Email == registrationDto.Email))
            {
                return new AuthResult { Success = false, Message = "User already exists" };
            }

            var user = new User
            {
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                Roles = new List<string>()
            };
            users.Add(user);
            return new AuthResult { Success = true, Message = "User registered successfully" };
        }

        public static AuthResult AssignRole(RoleAssignmentDto roleAssignmentDto)
        {
            var user = users.FirstOrDefault(u => u.Email == roleAssignmentDto.Email);
            if (user == null)
            {
                return new AuthResult { Success = false, Message = "User not found" };
            }

            if (!user.Roles.Contains(roleAssignmentDto.Role))
            {
                user.Roles.Add(roleAssignmentDto.Role);
            }

            return new AuthResult { Success = true, Message = "Role assigned successfully" };
        }

        private static string GenerateJwtToken(User user)
        {
            // Implement JWT token generation logic
            return "generated-jwt-token";
        }
    }
}
