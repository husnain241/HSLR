using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly HsrlDbContext _context;

        public ProjectsController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("projects")]
        public async Task<IActionResult> Index(ProjectStatus? status, ProjectType? type, int? domainId)
        {
            var query = _context.Projects
                .Include(p => p.Technologies.OrderBy(t => t.DisplayOrder))
                .Include(p => p.ProjectResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(p => p.ProjectType == type.Value);
            }

            if (domainId.HasValue)
            {
                query = query.Where(p => p.ProjectResearchAreas.Any(pra => pra.ResearchDomainId == domainId.Value));
            }

            var projects = await query
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.Year)
                .ToListAsync();

            ViewBag.SelectedStatus = status;
            ViewBag.SelectedType = type;
            ViewBag.SelectedDomainId = domainId;
            ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();

            return View(projects);
        }

        [Route("projects/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();

            var project = await _context.Projects
                .Include(p => p.Objectives.OrderBy(o => o.DisplayOrder))
                .Include(p => p.DataSources.OrderBy(d => d.DisplayOrder))
                .Include(p => p.MethodologySteps.OrderBy(m => m.DisplayOrder))
                .Include(p => p.Technologies.OrderBy(t => t.DisplayOrder))
                .Include(p => p.KeyFindings.OrderBy(k => k.DisplayOrder))
                .Include(p => p.ProjectResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (project == null)
            {
                return NotFound();
            }

            var domainIds = project.ProjectResearchAreas.Select(pra => pra.ResearchDomainId).ToList();
            var relatedPubs = await _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .Where(p => p.PublicationResearchAreas.Any(pra => domainIds.Contains(pra.ResearchDomainId)))
                .OrderByDescending(p => p.Year)
                .Take(3)
                .ToListAsync();

            ViewBag.RelatedPublications = relatedPubs;

            return View(project);
        }
    }
}
