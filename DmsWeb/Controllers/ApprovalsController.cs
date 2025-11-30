using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DmsWeb.Data;
using Microsoft.AspNetCore.Authorization;

namespace DmsWeb.Controllers
{
    [Authorize(Roles = "Admin")] // Bu controller'a sadece Admin rolü erişebilir
    public class ApprovalsController : Controller
    {
        private readonly AppDbContext _context;

        public ApprovalsController(AppDbContext context)
        {
            _context = context;
        }

        // Onay bekleyen belgeler listesi
        public IActionResult Index()
        {
            ViewBag.ActiveMenu = "Approvals";

            var waitingDocs = _context.Documents
                .Where(d => d.Status == "Onay Bekliyor")
                .OrderBy(d => d.CreatedAt)
                .ToList();

            return View(waitingDocs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            if (doc.Status == "Onay Bekliyor")
            {
                var approverName = User.Identity?.Name ?? "Admin";

                doc.Status = "Onaylandı";
                doc.ApprovedBy = approverName;
                doc.ApprovedAt = DateTime.Now;

                doc.RejectedBy = null;
                doc.RejectedAt = null;
                doc.RejectionReason = null;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id, string? reason)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null)
                return NotFound();

            if (doc.Status == "Onay Bekliyor")
            {
                var rejectorName = User.Identity?.Name ?? "Admin";

                doc.Status = "Reddedildi";
                doc.RejectedBy = rejectorName;
                doc.RejectedAt = DateTime.Now;
                doc.RejectionReason = reason;

                doc.ApprovedBy = null;
                doc.ApprovedAt = null;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
