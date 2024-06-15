namespace SchoolDutyManager.Models
{
    public class Duty
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Hours { get; set; }
        public List<int> AssignedPeople { get; set; }
    }
}
