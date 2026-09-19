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

            var featuredProjects = await _context.Projects
                .Include(p => p.ProjectResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .Include(p => p.Technologies.Take(3))
                .Where(p => p.Featured)
                .OrderBy(p => p.DisplayOrder)
                .Take(3)
                .ToListAsync();

            var featuredPublications = await _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .Include(p => p.PublicationResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .Where(p => p.Featured)
                .OrderByDescending(p => p.Year)
                .Take(4)
                .ToListAsync();

            var featuredPeople = await _context.People
                .Include(p => p.ResearchInterests)
                .Where(p => p.Featured && p.Category != PersonCategory.LabDirector)
                .OrderBy(p => p.DisplayOrder)
                .Take(4)
                .ToListAsync();

            var director = await _context.People
                .Include(p => p.ResearchInterests)
                .FirstOrDefaultAsync(p => p.Category == PersonCategory.LabDirector);

            var upcomingEvents = await _context.Events
                .Where(e => e.Status == EventStatus.Upcoming)
                .OrderBy(e => e.EventDate)
                .Take(3)
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
                FeaturedProjects = featuredProjects,
                FeaturedPublications = featuredPublications,
                FeaturedPeople = featuredPeople,
                Director = director,
                UpcomingEvents = upcomingEvents,
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
