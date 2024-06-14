namespace SchoolDutyManager.Models
{
    public class DutySwapRequestDto
    {
        public int OriginalDutyId { get; set; }
        public int RequestedDutyId { get; set; }
        public string RequestorEmail { get; set; }
        public string ResponderEmail { get; set; }
    }
}
