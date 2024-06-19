using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IUserService
    {
        User GetUserByEmail(string email);
        User GetUserById(int id);
        List<User> GetAllUsers();
        List<User> GetUsersByRole(string role); // Dodaj tę linię
        void AddUser(User user);
        void AssignRole(string email, string role);
        void DeleteUser(int id); // Dodaj tę linię
        bool IsTeacher(string email);
        bool IsAdmin(string email);
    }
}
