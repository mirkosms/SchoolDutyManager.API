using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> users;
        private readonly string filePath = "./Data/users.json";

        public UserService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json);
                System.Diagnostics.Debug.WriteLine($"Loaded {users.Count} users from {filePath}");
            }
            else
            {
                users = new List<User>();
            }
        }

        public User GetUserByEmail(string email)
        {
            return users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public List<User> GetUsersByRole(string role)
        {
            return users.Where(u => u.Roles.Contains(role)).ToList(); // Dodaj tę metodę
        }

        public void AddUser(User user)
        {
            users.Add(user);
            SaveToFile();
        }

        public void AssignRole(string email, string role)
        {
            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null && !user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                SaveToFile();
            }
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                users.Remove(user);
                SaveToFile();
            }
        }

        public bool IsTeacher(string email)
        {
            var user = users.FirstOrDefault(u => u.Email == email);
            return user?.Roles.Contains("Teacher") ?? false;
        }

        public bool IsAdmin(string email)
        {
            var user = users.FirstOrDefault(u => u.Email == email);
            return user?.Roles.Contains("Admin") ?? false;
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
