namespace DmsWeb.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
