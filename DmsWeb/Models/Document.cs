namespace DmsWeb.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Number { get; set; } = "";
        public string Title { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "";

        public string StoredFileName { get; set; } = "";
        public string? OriginalFileName { get; set; }
        public string? ExternalContent { get; set; }  // API'den gelen body


        public bool IsPublic { get; set; }  

        // 🔽 Onay bilgileri
        public string? ApprovedBy { get; set; }       // Onaylayan kişi
        public DateTime? ApprovedAt { get; set; }     // Onay tarihi
        public string? RejectedBy { get; set; }       // Reddeden kişi
        public DateTime? RejectedAt { get; set; }     // Red tarihi
        public string? RejectionReason { get; set; }  // Red sebebi
    }
}
