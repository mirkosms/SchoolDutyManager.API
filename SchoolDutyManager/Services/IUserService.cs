using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IUserService
    {
        User GetUserByEmail(string email);
        List<User> GetAllUsers();
        void AddUser(User user);
        void AssignRole(string email, string role);
        bool IsTeacher(string email);
        bool IsAdmin(string email);
    }
}
