// Models/AppUser.cs
using System.ComponentModel.DataAnnotations;

namespace DmsWeb.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required, MaxLength(100)]
        public string FullName { get; set; } = "";

        // "Admin" / "User"
        [Required, MaxLength(20)]
        public string Role { get; set; } = "User";

        // Pasif etme için
        public bool IsActive { get; set; } = true;
    }
}
