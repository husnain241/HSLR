using HSLR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class ResearchController : Controller
    {
        private readonly HsrlDbContext _context;

        public ResearchController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("research")]
        public async Task<IActionResult> Index()
        {
            var domains = await _context.ResearchDomains
                .Include(d => d.FocusTopics.OrderBy(t => t.DisplayOrder))
                .Include(d => d.ProjectResearchAreas)
                    .ThenInclude(pra => pra.Project)
                .Include(d => d.PublicationResearchAreas)
                    .ThenInclude(pra => pra.Publication)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            var capabilities = await _context.Capabilities
                .Include(c => c.Highlights.OrderBy(h => h.DisplayOrder))
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            ViewBag.Capabilities = capabilities;
            return View(domains);
        }
    }
}
