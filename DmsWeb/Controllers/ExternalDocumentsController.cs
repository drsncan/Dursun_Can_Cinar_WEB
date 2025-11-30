using DmsWeb.Data;
using DmsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;

namespace DmsWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalDocumentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly HttpClient _http;

        public ExternalDocumentsController(AppDbContext db)
        {
            _db = db;
            _http = new HttpClient();
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import()
        {
            try
            {
                // Örnek ücretsiz API
                var apiUrl = "https://jsonplaceholder.typicode.com/posts";

                var items = await _http.GetFromJsonAsync<List<ExternalDocDto>>(apiUrl);
                if (items == null || items.Count == 0)
                    return BadRequest(new { message = "API'den veri alınamadı." });

                // Mevcut belge numaralarını çek -> unique çakışmasını engelle
                var existingNumbers = new HashSet<string>(
                    await _db.Documents
                             .Select(d => d.Number)
                             .ToListAsync()
                );

                int added = 0;

                foreach (var p in items.Take(10))       // İlk 10 post
                {
                    var number = $"API-{p.Id}";

                    // Aynı numara zaten varsa atla
                    if (existingNumbers.Contains(number))
                        continue;

                    existingNumbers.Add(number);

                    var doc = new Document
                    {
                        Number = number,
                        Title = p.Title ?? "(Başlıksız API kaydı)",
                        CreatedBy = "API",
                        CreatedAt = DateTime.Now,
                        Status = "Taslak",
                        StoredFileName = string.Empty,
                        OriginalFileName = $"api-{p.Id}.txt",
                        ExternalContent = p.Body

                        // Eğer modelde böyle alanlar varsa, güvenli default verelim:
                        // IsPublic = true
                    };

                    _db.Documents.Add(doc);
                    added++;
                }

                if (added == 0)
                    return Ok(new { message = "Yeni eklenecek belge bulunamadı (tamamı zaten var olabilir)." });

                await _db.SaveChangesAsync();

                return Ok(new { message = $"{added} belge başarıyla içe aktarıldı." });
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı hatası – iç mesajı da göster
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Veritabanı hatası: " + inner });
            }
            catch (Exception ex)
            {
                // Diğer hatalar
                return StatusCode(500, new { message = "Sunucu hatası: " + ex.Message });
            }
        }
    }

    public class ExternalDocDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
    }
}
