namespace SchoolDutyManager.Models
{
    public class Duty
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Hours { get; set; }
        public List<string> AssignedPeople { get; set; } = new List<string>();
    }
}
