using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DmsWeb.Models
{
    public class DocumentEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Belge No")]
        public string Number { get; set; } = "";

        [Required]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = "";

        [Required]
        [Display(Name = "Oluşturan")]
        public string CreatedBy { get; set; } = "";

        [Required]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "";

        [Display(Name = "Mevcut Dosya")]
        public string ExistingFileName { get; set; } = "";

        public bool IsPublic { get; set; }

        [Display(Name = "Yeni Dosya (İstersen Değiştir)")]
        public IFormFile? NewFile { get; set; }
    }
}
