using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DmsWeb.Data;
using DmsWeb.Models;

namespace DmsWeb.Controllers
{
    [Route("api/documents")]       // ← ÖNEMLİ: URL tam olarak /api/documents olacak
    [ApiController]
    [Authorize(Roles = "Admin")]   // API sadece Admin'e açık
    public class DocumentsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/documents
        [HttpGet]
        public IActionResult GetAll()
        {
            var docs = _context.Documents
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Number = d.Number,
                    Title = d.Title,
                    CreatedBy = d.CreatedBy,
                    CreatedAt = d.CreatedAt,
                    Status = d.Status
                })
                .ToList();

            return Ok(docs);
        }

        // GET: /api/documents/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var d = _context.Documents
                .Where(x => x.Id == id)
                .Select(x => new DocumentDto
                {
                    Id = x.Id,
                    Number = x.Number,
                    Title = x.Title,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status
                })
                .FirstOrDefault();

            if (d == null)
                return NotFound();

            return Ok(d);
        }
    }
}
