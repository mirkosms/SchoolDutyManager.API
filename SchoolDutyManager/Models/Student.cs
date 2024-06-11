namespace SchoolDutyManager.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Classes { get; set; }
        public List<string> Duties { get; set; }
    }
}
