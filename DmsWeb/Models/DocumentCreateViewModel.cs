using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DmsWeb.Models
{
    public class DocumentCreateViewModel
    {
        [Required(ErrorMessage = "Belge numarası zorunludur.")]
        [Display(Name = "Belge No")]
        public string Number { get; set; } = "";

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Durum seçmelisiniz.")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Taslak";

        [Display(Name = "Belge Dosyası")]
        public IFormFile? File { get; set; }

        public bool IsPublic { get; set; } = false;
    }
}
