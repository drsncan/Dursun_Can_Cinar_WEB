namespace DmsWeb.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string Actor { get; set; } = "";     // username
        public string Event { get; set; } = "";     // "DocumentCreated", "DocumentApproved" vs
        public string? Detail { get; set; }         // serbest açıklama

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
