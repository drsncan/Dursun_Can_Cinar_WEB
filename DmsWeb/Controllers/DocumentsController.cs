using DmsWeb.Data;
using DmsWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;

namespace DmsWeb.Controllers
{
    [Authorize] // Bu controller'daki tüm yöntemler için login zorunlu
    public class DocumentsController : Controller
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(string? search, string? status, string? sort, string? order, bool? onlyMine)
        {
            ViewBag.ActiveMenu = "Documents";

            var query = _context.Documents.AsQueryable();

            // 🔍 Arama
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.Number.Contains(search) ||
                    d.Title.Contains(search));
            }

            // 🎯 Durum filtresi
            if (!string.IsNullOrWhiteSpace(status) && status != "Hepsi")
            {
                query = query.Where(d => d.Status == status);
            }

            // 🧭 Sıralama (senin daha önce yazdığın switch burada dursun)
            // ...

            // 🔐 KULLANICI FİLTRESİ
            var isAdmin = User.IsInRole("Admin");
            var currentUserName = User.Identity?.Name;

            // Admin ise IsPublic filtresi uygulama (her şeyi görsün)
            if (!isAdmin)
            {
                if (!string.IsNullOrEmpty(currentUserName))
                {
                    // Kullanıcı kendi belgelerini HER ZAMAN görsün,
                    // diğerlerinin sadece IsPublic = true olanlarını görsün
                    query = query.Where(d =>
                        d.CreatedBy == currentUserName || d.IsPublic);
                }
                else
                {
                    // teorik olarak login olmayan gelmez ama dursun
                    query = query.Where(d => d.IsPublic);
                }
            }
            else
            {
                // İstersen admin için onlyMine desteğini ayrı tutabilirsin:
                if (onlyMine == true && !string.IsNullOrEmpty(currentUserName))
                {
                    query = query.Where(d => d.CreatedBy == currentUserName);
                }
            }


            var model = query.ToList();

            ViewBag.Search = search;
            ViewBag.Status = status ?? "Hepsi";
            ViewBag.Sort = sort ?? "date";
            ViewBag.Order = order ?? "desc";
            ViewBag.OnlyMine = onlyMine ?? (!isAdmin); // admin değilse default true

            return View(model);
        }
            

        // API için:
        [NonAction]
        public static IQueryable<Document> GetAllStatic(AppDbContext context)
            => context.Documents.AsQueryable();

        // GET: Create
        [HttpGet]
        [Authorize] // Her giriş yapmış kullanıcı belge oluşturabilsin
        public IActionResult Create()
        {
            ViewBag.ActiveMenu = "Documents";

            var vm = new DocumentCreateViewModel
            {
                Status = "Taslak",
                
                
            };

            return View(vm);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentCreateViewModel model)
        {
            ViewBag.ActiveMenu = "Documents";

            if (!ModelState.IsValid)
                return View(model);

            string storedFileName = "";
            string originalFileName = "";

            if (model.File != null && model.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                originalFileName = model.File.FileName;
                storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";

                var filePath = Path.Combine(uploadsFolder, storedFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                model.File.CopyTo(stream);
            }

            // 🔴 ÖNEMLİ: Belgeyi oluşturanı login kullanıcıdan al
            var currentUserName = User.Identity?.Name ?? "Bilinmiyor";

            var doc = new Document
            {
                Number = model.Number,
                Title = model.Title,
                CreatedBy = currentUserName,   // eskiden model.CreatedBy idi
                CreatedAt = DateTime.Now,
                Status = model.Status,
                StoredFileName = storedFileName,
                OriginalFileName = originalFileName,
                IsPublic = model.IsPublic
            };

            _context.Documents.Add(doc);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Download(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null || string.IsNullOrEmpty(doc.StoredFileName))
                return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, doc.StoredFileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var downloadName = string.IsNullOrEmpty(doc.OriginalFileName)
                ? doc.StoredFileName
                : doc.OriginalFileName;

            return PhysicalFile(filePath, "application/octet-stream", downloadName);
        }

        public IActionResult Preview(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null || string.IsNullOrEmpty(doc.StoredFileName))
                return NotFound();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var filePath = Path.Combine(uploadsFolder, doc.StoredFileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                // PDF odaklı gidiyorsan sabitleyebilirsin
                contentType = "application/pdf";
            }

            var downloadName = string.IsNullOrEmpty(doc.OriginalFileName)
                ? Path.GetFileName(filePath)
                : doc.OriginalFileName;

            Response.Headers[HeaderNames.ContentDisposition] =
                $"inline; filename=\"{downloadName}\"";

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, contentType);
        }

        public IActionResult Details(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            ViewBag.ActiveMenu = "Documents";
            return View(doc);
        }

        // GET: Edit (Sadece Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            ViewBag.ActiveMenu = "Documents";

            var vm = new DocumentEditViewModel
            {
                Id = doc.Id,
                Number = doc.Number,
                Title = doc.Title,
                CreatedBy = doc.CreatedBy,
                Status = doc.Status,
                ExistingFileName = doc.OriginalFileName,
                IsPublic = doc.IsPublic
            };

            return View(vm);
        }

        // POST: Edit (Sadece Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DocumentEditViewModel model)
        {
            ViewBag.ActiveMenu = "Documents";

            if (!ModelState.IsValid)
                return View(model);

            var doc = _context.Documents.FirstOrDefault(d => d.Id == model.Id);
            if (doc == null)
                return NotFound();

            // Metin alanlarını güncelle
            doc.Number = model.Number;
            doc.Title = model.Title;
            doc.CreatedBy = model.CreatedBy;
            doc.Status = model.Status;
            doc.IsPublic = model.IsPublic;
            // CreatedAt aynen kalsın (ilk oluşturma tarihi)

            // Yeni dosya seçilmişse eskisini değiştir
            if (model.NewFile != null && model.NewFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                // Eski dosyayı sil (varsa)
                if (!string.IsNullOrEmpty(doc.StoredFileName))
                {
                    var oldPath = Path.Combine(uploadsFolder, doc.StoredFileName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var newStoredName = $"{Guid.NewGuid()}{Path.GetExtension(model.NewFile.FileName)}";
                var newPath = Path.Combine(uploadsFolder, newStoredName);
                using var stream = new FileStream(newPath, FileMode.Create);
                model.NewFile.CopyTo(stream);

                doc.StoredFileName = newStoredName;
                doc.OriginalFileName = model.NewFile.FileName;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = doc.Id });
        }

        // POST: Delete (Sadece Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            // Önce dosyayı sil (varsa)
            if (!string.IsNullOrEmpty(doc.StoredFileName))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                var filePath = Path.Combine(uploadsFolder, doc.StoredFileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Sonra veritabanı kaydını sil
            _context.Documents.Remove(doc);
            _context.SaveChanges();

            // Belgeler listesine dön
            return RedirectToAction(nameof(Index));
        }

        // Onaya Gönder (login olan herkes yapabilsin diye ekstra rol kısıtı yok)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendForApproval(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            // Sadece Taslak ise onaya gönder
            if (doc.Status == "Taslak")
            {
                doc.Status = "Onay Bekliyor";
                // Şimdilik sabit, ileride login kullanıcıya göre değiştiririz
                doc.ApprovedBy = null;
                doc.ApprovedAt = null;
                doc.RejectedBy = null;
                doc.RejectedAt = null;
                doc.RejectionReason = null;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
