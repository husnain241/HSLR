using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly HsrlDbContext _context;

        public DashboardController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalInquiries = await _context.CollaborationInquiries.CountAsync();
            var newInquiries = await _context.CollaborationInquiries.CountAsync(i => i.Status == InquiryStatus.New && !i.IsSpam);
            var totalPublications = await _context.Publications.CountAsync();
            var totalProjects = await _context.Projects.CountAsync();
            var activeProjects = await _context.Projects.CountAsync(p => p.Status == ProjectStatus.Active);
            var totalPeople = await _context.People.CountAsync();
            var totalEvents = await _context.Events.CountAsync();
            var totalGallery = await _context.GalleryItems.CountAsync();
            var totalDomains = await _context.ResearchDomains.CountAsync();

            var recentInquiries = await _context.CollaborationInquiries
                .Include(i => i.ResearchDomain)
                .OrderByDescending(i => i.SubmittedAtUtc)
                .Take(5)
                .ToListAsync();

            var recentProjects = await _context.Projects
                .OrderByDescending(p => p.Year)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalInquiries = totalInquiries;
            ViewBag.NewInquiries = newInquiries;
            ViewBag.TotalPublications = totalPublications;
            ViewBag.TotalProjects = totalProjects;
            ViewBag.ActiveProjects = activeProjects;
            ViewBag.TotalPeople = totalPeople;
            ViewBag.TotalEvents = totalEvents;
            ViewBag.TotalGallery = totalGallery;
            ViewBag.TotalDomains = totalDomains;
            ViewBag.RecentInquiries = recentInquiries;
            ViewBag.RecentProjects = recentProjects;

            return View();
        }
    }
}
