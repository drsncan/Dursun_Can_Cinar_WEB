using System.Collections.Generic;

namespace DmsWeb.Models
{
    public class DashboardViewModel
    {
        public int TotalDocuments { get; set; }
        public int DraftCount { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int MyDocuments { get; set; }

        public List<Document> RecentDocuments { get; set; } = new();
        public List<Document> PendingDocuments { get; set; } = new();
    }
}
