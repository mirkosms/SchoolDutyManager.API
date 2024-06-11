namespace SchoolDutyManager.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<string> AssignedPeople { get; set; } = new List<string>();
    }
}
