using System.Security.Claims;
using DmsWeb.Data;
using DmsWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DmsWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ---- LOGIN ----
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre zorunludur.");
                return View();
            }

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password); // DEMO için düz karşılaştırma

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                return View();
            }

            await SignInUser(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ---- REGISTER ----
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Tüm alanları doldurun.");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Şifreler uyuşmuyor.");
                return View(model);
            }

            // Kullanıcı adı daha önce alınmış mı?
            var exists = _context.Users.Any(u => u.Username == model.Username);
            if (exists)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten kullanılıyor.");
                return View(model);
            }

            // Yeni kullanıcı (rol: User)
            var user = new AppUser
            {
                Username = model.Username,
                FullName = model.FullName,
                Password = model.Password,   // Gerçek projede HASH kullanılmalı
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Kayıttan sonra otomatik login
            await SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        // ---- LOGOUT ----
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Ortak: cookie oluşturma
        private async Task SignInUser(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FullName", user.FullName ?? user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });
        }
    }
}
