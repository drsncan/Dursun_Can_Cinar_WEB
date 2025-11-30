using DmsWeb.Data;
using DmsWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmsWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Settings
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.ActiveMenu = "Settings";

            // Tek satırlık ayar tablosu: ilk kaydı çek
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new SystemSettings
                {
                    SystemName = "Döküman Yönetim Sistemi",
                    InstitutionName = "Ankara Üniversitesi",
                    Theme = "dark",
                    MaxUploadSizeMb = 20,
                    AllowedExtensions = ".pdf,.docx,.xlsx,.pptx"
                };

                _context.SystemSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            var vm = new SystemSettingsViewModel
            {
                Id = settings.Id,
                SystemName = settings.SystemName,
                InstitutionName = settings.InstitutionName,
                Theme = settings.Theme,
                ExistingLogoPath = settings.LogoPath,
                MaxUploadSizeMb = settings.MaxUploadSizeMb,
                AllowedExtensions = settings.AllowedExtensions
            };

            return View(vm);
        }

        // POST: /Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SystemSettingsViewModel model)
        {
            ViewBag.ActiveMenu = "Settings";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var settings = await _context.SystemSettings
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (settings == null)
            {
                settings = new SystemSettings();
                _context.SystemSettings.Add(settings);
            }

            settings.SystemName = model.SystemName;
            settings.InstitutionName = model.InstitutionName;
            settings.Theme = model.Theme;
            settings.MaxUploadSizeMb = model.MaxUploadSizeMb;
            settings.AllowedExtensions = model.AllowedExtensions;

            // Logo yükleme
            if (model.LogoFile != null && model.LogoFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "logos");
                Directory.CreateDirectory(uploadsRoot);

                var extension = Path.GetExtension(model.LogoFile.FileName);
                var fileName = $"logo_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.LogoFile.CopyToAsync(stream);
                }

                settings.LogoPath = "/uploads/logos/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sistem ayarları başarıyla kaydedildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}

