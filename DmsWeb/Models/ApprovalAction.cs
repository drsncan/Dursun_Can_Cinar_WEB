namespace DmsWeb.Models
{
    public class ApprovalAction
    {
        public int Id { get; set; }

        public int ApprovalRequestId { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; } = null!;

        public string Actor { get; set; } = "";  // admin/user username
        public string Action { get; set; } = ""; // Approve / Reject
        public string? Comment { get; set; }

        public DateTime ActionAt { get; set; } = DateTime.UtcNow;
    }
}
