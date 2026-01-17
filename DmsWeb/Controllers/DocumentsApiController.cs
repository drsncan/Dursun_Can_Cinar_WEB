using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using DmsWeb.Data;
using DmsWeb.Models;

namespace DmsWeb.Controllers
{
    [Route("api/documents")]                 // /api/documents
    [ApiController]
    [Authorize(Roles = "Admin")]              // API sadece Admin
    public class DocumentsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsApiController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: /api/documents
        // =========================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        // =========================
        // GET: /api/documents/{id}
        // =========================
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        // =========================
        // POST: /api/documents
        // =========================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] DocumentCreateViewModel model)

        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doc = new Document
            {
                Number = model.Number,
                Title = model.Title,
                CreatedBy = User.Identity?.Name ?? "Admin",
                CreatedAt = DateTime.Now,
                Status = model.Status ?? "Taslak",
                IsPublic = model.IsPublic
            };

            _context.Documents.Add(doc);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = doc.Id }, doc);
        }

        // =========================
        // PUT: /api/documents/{id}
        // =========================
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, [FromBody] DocumentEditViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            doc.Number = model.Number;
            doc.Title = model.Title;
            doc.Status = model.Status;
            doc.IsPublic = model.IsPublic;

            _context.SaveChanges();

            return NoContent();
        }

        // =========================
        // DELETE: /api/documents/{id}
        // =========================
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            _context.Documents.Remove(doc);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
