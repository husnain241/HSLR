using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class TeamController : Controller
    {
        private readonly HsrlDbContext _context;

        public TeamController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("research-family")]
        public async Task<IActionResult> Index(PersonCategory? category)
        {
            var query = _context.People
                .Include(p => p.ResearchInterests.OrderBy(i => i.DisplayOrder))
                .Include(p => p.PersonResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .AsQueryable();

            if (category.HasValue)
            {
                query = query.Where(p => p.Category == category.Value);
            }

            var teamMembers = await query
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayOrder)
                .ToListAsync();

            ViewBag.SelectedCategory = category;
            return View(teamMembers);
        }

        [Route("professor")]
        public async Task<IActionResult> Director()
        {
            var director = await _context.People
                .Include(p => p.ResearchInterests.OrderBy(i => i.DisplayOrder))
                .Include(p => p.PersonResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .FirstOrDefaultAsync(p => p.Category == PersonCategory.LabDirector);

            if (director == null)
            {
                return NotFound();
            }

            // Publications authored by or associated with director
            var publications = await _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .Include(p => p.PublicationResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .Where(p => p.Authors.Any(a => a.AuthorName.Contains("Boota") || a.AuthorName.Contains("Waseem")))
                .OrderByDescending(p => p.Year)
                .ToListAsync();

            // Projects led or supervised
            var projects = await _context.Projects
                .Include(p => p.ProjectResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            ViewBag.Publications = publications;
            ViewBag.Projects = projects;

            return View(director);
        }
    }
}
