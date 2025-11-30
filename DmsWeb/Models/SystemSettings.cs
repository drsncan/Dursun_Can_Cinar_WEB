using System.ComponentModel.DataAnnotations;

namespace DmsWeb.Models
{
    public class SystemSettings
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string SystemName { get; set; } = "Döküman Yönetim Sistemi";

        [Required, MaxLength(200)]
        public string InstitutionName { get; set; } = "Kurum Adı";

        [MaxLength(20)]
        public string Theme { get; set; } = "dark";
        // ileride "light"/"dark" vs.

        [MaxLength(260)]
        public string? LogoPath { get; set; }   // wwwroot/uploads içindeki logo dosyası ismi

        // Dosya / güvenlik vs. için şimdiden alan açalım:
        public int MaxUploadSizeMb { get; set; } = 20;    // Maks dosya boyutu (MB)

        [MaxLength(200)]
        public string AllowedExtensions { get; set; } = ".pdf,.docx,.xlsx,.pptx";
    }
}
