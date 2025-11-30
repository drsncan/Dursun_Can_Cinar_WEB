// Controllers/UsersController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DmsWeb.Data;
using DmsWeb.Models;


namespace DmsWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Liste
        public IActionResult Index()
        {
            ViewBag.ActiveMenu = "Users";

            var users = _context.Users
                .OrderBy(u => u.Username)
                .ToList();

            return View(users);
        }

        // Bir kullanıcının belgeleri
        public IActionResult Documents(int id)
        {
            ViewBag.ActiveMenu = "Users";

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            // CreatedBy'ı username olarak tuttuğumuzu varsayıyorum.
            var docs = _context.Documents
                .Where(d => d.CreatedBy == user.Username)
                .OrderByDescending(d => d.CreatedAt)
                .ToList();

            var vm = new UserDocumentsViewModel
            {
                User = user,
                Documents = docs
            };

            return View(vm);
        }

        // Rol değiştir (User <-> Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleRole(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            var currentUserName = User.Identity?.Name;

            // Kendini Admin->User yapmayı engelle (istenmez genelde)
            if (user.Username == currentUserName)
            {
                TempData["Error"] = "Kendi rolünü değiştiremezsin.";
                return RedirectToAction(nameof(Index));
            }

            user.Role = user.Role == "Admin" ? "User" : "Admin";
            _context.SaveChanges();

            TempData["Message"] = $"{user.Username} kullanıcısının rolü {user.Role} olarak güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // Aktif / Pasif
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            var currentUserName = User.Identity?.Name;
            if (user.Username == currentUserName)
            {
                TempData["Error"] = "Kendi hesabını pasif yapamazsın.";
                return RedirectToAction(nameof(Index));
            }

            user.IsActive = !user.IsActive;
            _context.SaveChanges();

            TempData["Message"] = $"{user.Username} {(user.IsActive ? "aktif edildi" : "pasif hale getirildi")}.";
            return RedirectToAction(nameof(Index));
        }

        // Sil (hard delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            var currentUserName = User.Identity?.Name;
            if (user.Username == currentUserName)
            {
                TempData["Error"] = "Kendi hesabını silemezsin.";
                return RedirectToAction(nameof(Index));
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Message"] = $"{user.Username} silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
