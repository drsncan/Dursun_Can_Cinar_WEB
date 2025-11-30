namespace DmsWeb.Models
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = "";
        public string Title { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "";
    }
}
