namespace SchoolDutyManager.Models
{
    public class DutySwap
    {
        public int Id { get; set; }
        public int OriginalDutyId { get; set; }
        public int RequestedDutyId { get; set; }
        public string Status { get; set; } = "Pending"; // Status: Pending, Approved, Rejected
        public int InitiatingStudentId { get; set; }
        public int RespondingStudentId { get; set; }
    }
}
