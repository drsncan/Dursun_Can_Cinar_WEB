using System.Diagnostics;
using System.Linq;
using DmsWeb.Data;
using DmsWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DmsWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ActiveMenu = "Home";

            var docsQuery = _context.Documents.AsQueryable();

            var model = new DashboardViewModel
            {
                TotalDocuments = docsQuery.Count(),
                DraftCount = docsQuery.Count(d => d.Status == "Taslak"),
                PendingCount = docsQuery.Count(d => d.Status == "Onay Bekliyor"),
                ApprovedCount = docsQuery.Count(d => d.Status == "Onaylandý"),
                RejectedCount = docsQuery.Count(d => d.Status == "Reddedildi")
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                var username = User.Identity!.Name!;

                model.MyDocuments = docsQuery.Count(d => d.CreatedBy == username);

                model.RecentDocuments = docsQuery
                    .OrderByDescending(d => d.CreatedAt)
                    .Take(5)
                    .ToList();

                model.PendingDocuments = docsQuery
                    .Where(d => d.Status == "Onay Bekliyor")
                    .OrderBy(d => d.CreatedAt)
                    .Take(5)
                    .ToList();
            }

            return View(model);
        }

        
    }
}
