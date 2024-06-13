namespace SchoolDutyManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RoleAssignmentDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
