namespace SchoolDutyManager.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public List<string> Duties { get; set; } = new List<string>();
    }
}
