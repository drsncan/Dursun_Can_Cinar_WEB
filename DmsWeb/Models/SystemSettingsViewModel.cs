using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DmsWeb.Models
{
    public class SystemSettingsViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Sistem Adı")]
        public string SystemName { get; set; } = "";

        [Required, StringLength(200)]
        [Display(Name = "Kurum Adı")]
        public string InstitutionName { get; set; } = "";

        [Required, StringLength(20)]
        [Display(Name = "Tema")]
        public string Theme { get; set; } = "light";

        [Display(Name = "Mevcut Logo")]
        public string? ExistingLogoPath { get; set; }

        [Display(Name = "Logo (Yeni)")]
        public IFormFile? LogoFile { get; set; }

        [Range(1, 200)]
        [Display(Name = "Maks. Dosya Boyutu (MB)")]
        public int MaxUploadSizeMb { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "İzin Verilen Uzantılar")]
        public string AllowedExtensions { get; set; } = "";
    }
}

