using Microsoft.IdentityModel.Tokens;
using SchoolDutyManager.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public static class AuthService
    {
        private static List<User> users;
        private static readonly string secretKey = "this_is_a_longer_secret_key_32bytes!";
        private static readonly string filePath = "./Data/users.json";

        static AuthService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json);
            }
            else
            {
                users = new List<User>();
            }
        }

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
            SaveToFile();
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
            SaveToFile();
            return new AuthResult { Success = true, Message = "Role assigned successfully" };
        }

        public static User GetUserByEmail(string email)
        {
            return users.Find(u => u.Email == email);
        }

        public static List<User> GetAllUsers()
        {
            return users;
        }

        private static string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var role = user.Roles.FirstOrDefault();
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role), "User role cannot be null or empty.");
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
