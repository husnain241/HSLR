using HSLR.Data;
using HSLR.Models;
using HSLR.Models.Entities;
using HSLR.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HSLR.Controllers
{
    public class HomeController : Controller
    {
        private readonly HsrlDbContext _context;

        public HomeController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var config = await _context.SiteConfigs.FirstOrDefaultAsync() ?? new SiteConfig();

            var domains = await _context.ResearchDomains
                .Include(d => d.FocusTopics)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            var capabilities = await _context.Capabilities
                .Include(c => c.Highlights.OrderBy(h => h.DisplayOrder))
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var totalPubs = await _context.Publications.CountAsync();
            var totalProjects = await _context.Projects.CountAsync();
            var totalScholars = await _context.People.CountAsync(p => p.Status == PersonStatus.Active);
            var totalDomains = domains.Count;

            var model = new HomeIndexViewModel
            {
                Config = config,
                ResearchDomains = domains,
                Capabilities = capabilities,
                TotalPublications = totalPubs,
                TotalProjects = totalProjects,
                TotalScholars = totalScholars,
                TotalDomains = totalDomains
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
