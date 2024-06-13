using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolDutyManager.Services
{
    public static class AuthService
    {
        private static List<User> users = new List<User>();

        public static AuthResult RegisterUser(UserRegistrationDto registrationDto)
        {
            if (users.Any(u => u.Email == registrationDto.Email))
            {
                return new AuthResult { Success = false, Message = "User already exists" };
            }

            var user = new User
            {
                Id = users.Count > 0 ? users[^1].Id + 1 : 1,
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                Role = "User"
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

            user.Role = roleAssignmentDto.Role;
            return new AuthResult { Success = true, Message = "Role assigned successfully" };
        }

        public static AuthResult Authenticate(string email, string password, string secretKey)
        {
            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return new AuthResult { Success = false, Message = "Invalid email or password" };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResult { Success = true, Token = tokenString };
        }
    }
}
