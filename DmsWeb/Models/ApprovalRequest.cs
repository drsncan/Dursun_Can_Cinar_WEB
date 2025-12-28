namespace DmsWeb.Models
{
    public class ApprovalRequest
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; } = null!;

        public string RequestedBy { get; set; } = ""; // username ya da fullname
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        // Pending / Approved / Rejected
        public string Status { get; set; } = "Pending";

        public List<ApprovalAction> Actions { get; set; } = new();
    }
}
