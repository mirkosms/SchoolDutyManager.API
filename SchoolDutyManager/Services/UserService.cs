using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolDutyManager.Services
{
    public class UserService : IUserService
    {
        private static readonly List<User> users = new List<User>();

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
            users.Add(user);
        }

        public void AssignRole(string email, string role)
        {
            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null && !user.Roles.Contains(role))
            {
                user.Roles.Add(role);
            }
        }
    }
}
