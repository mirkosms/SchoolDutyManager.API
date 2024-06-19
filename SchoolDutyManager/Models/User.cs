namespace SchoolDutyManager.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public string FirstName { get; set; }  // Dodane pole
        public string LastName { get; set; }   // Dodane pole
    }
}
