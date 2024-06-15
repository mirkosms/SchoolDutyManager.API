namespace SchoolDutyManager.Models
{
    public class DutySwapRequestDto
    {
        public int OriginalDutyId { get; set; }
        public int RequestedDutyId { get; set; }
        public int InitiatingStudentId { get; set; }
        public int RespondingStudentId { get; set; }
    }
}
