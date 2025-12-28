namespace DmsWeb.Models
{
    public class DocumentFile
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; } = null!;

        public string StoredFileName { get; set; } = "";
        public string OriginalFileName { get; set; } = "";

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // İstersen username ile tut (mevcut yapına uyumlu)
        public string UploadedBy { get; set; } = "";
    }
}
