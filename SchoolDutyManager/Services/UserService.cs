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

        public List<User> GetAllUsers()
        {
            return users;
        }

        public void AddUser(User user)
        {
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1; // Ustawienie ID użytkownika
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

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
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

        public List<User> GetUsersByRole(string role)
        {
            return users.Where(u => u.Roles.Contains(role)).ToList();
        }

        public void UpdateUser(User updatedUser)
        {
            var user = GetUserById(updatedUser.Id);
            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Password = updatedUser.Password; // Aktualizacja hasła
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
