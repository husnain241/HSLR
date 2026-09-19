using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly HsrlDbContext _context;

        public ProjectsController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.ProjectResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.Year)
                .ToListAsync();

            return View(projects);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
            return View(new Project { Year = DateTime.UtcNow.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project, string? objectivesList, string? dataSourcesList, string? technologiesList, string? findingsList, int[]? domainIds)
        {
            if (string.IsNullOrWhiteSpace(project.Slug))
            {
                project.Slug = project.ShortTitle.ToLowerInvariant().Replace(" ", "-").Replace(".", "");
            }

            if (await _context.Projects.AnyAsync(p => p.Slug == project.Slug))
            {
                ModelState.AddModelError("Slug", "A project with this slug already exists.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
                return View(project);
            }

            if (!string.IsNullOrWhiteSpace(objectivesList))
            {
                var objs = objectivesList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var o in objs) project.Objectives.Add(new ProjectObjective { Objective = o, DisplayOrder = order++ });
            }

            if (!string.IsNullOrWhiteSpace(dataSourcesList))
            {
                var srcs = dataSourcesList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var s in srcs) project.DataSources.Add(new ProjectDataSource { SourceName = s, DisplayOrder = order++ });
            }

            if (!string.IsNullOrWhiteSpace(technologiesList))
            {
                var techs = technologiesList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var t in techs) project.Technologies.Add(new ProjectTechnology { TechnologyName = t, DisplayOrder = order++ });
            }

            if (!string.IsNullOrWhiteSpace(findingsList))
            {
                var fnds = findingsList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var f in fnds) project.KeyFindings.Add(new ProjectKeyFinding { Finding = f, DisplayOrder = order++ });
            }

            if (domainIds != null)
            {
                foreach (var did in domainIds)
                {
                    project.ProjectResearchAreas.Add(new ProjectResearchArea { ResearchDomainId = did });
                }
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Project created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Objectives)
                .Include(p => p.DataSources)
                .Include(p => p.Technologies)
                .Include(p => p.KeyFindings)
                .Include(p => p.ProjectResearchAreas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return NotFound();

            ViewBag.ObjectivesList = string.Join("\n", project.Objectives.Select(o => o.Objective));
            ViewBag.DataSourcesList = string.Join(", ", project.DataSources.Select(d => d.SourceName));
            ViewBag.TechnologiesList = string.Join(", ", project.Technologies.Select(t => t.TechnologyName));
            ViewBag.FindingsList = string.Join("\n", project.KeyFindings.Select(k => k.Finding));
            ViewBag.SelectedDomainIds = project.ProjectResearchAreas.Select(pra => pra.ResearchDomainId).ToList();
            ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project model, string? objectivesList, string? dataSourcesList, string? technologiesList, string? findingsList, int[]? domainIds)
        {
            if (id != model.Id) return NotFound();

            var project = await _context.Projects
                .Include(p => p.Objectives)
                .Include(p => p.DataSources)
                .Include(p => p.Technologies)
                .Include(p => p.KeyFindings)
                .Include(p => p.ProjectResearchAreas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ObjectivesList = objectivesList;
                ViewBag.DataSourcesList = dataSourcesList;
                ViewBag.TechnologiesList = technologiesList;
                ViewBag.FindingsList = findingsList;
                ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
                return View(model);
            }

            project.Title = model.Title;
            project.ShortTitle = model.ShortTitle;
            project.Subtitle = model.Subtitle;
            project.Slug = model.Slug;
            project.Status = model.Status;
            project.ProjectType = model.ProjectType;
            project.Year = model.Year;
            project.StudyArea = model.StudyArea;
            project.Country = model.Country;
            project.Challenge = model.Challenge;
            project.Impact = model.Impact;
            project.Featured = model.Featured;
            project.ImageUrl = model.ImageUrl;
            project.DisplayOrder = model.DisplayOrder;

            // Objectives
            _context.ProjectObjectives.RemoveRange(project.Objectives);
            if (!string.IsNullOrWhiteSpace(objectivesList))
            {
                var objs = objectivesList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var o in objs) project.Objectives.Add(new ProjectObjective { Objective = o, DisplayOrder = order++ });
            }

            // Data sources
            _context.ProjectDataSources.RemoveRange(project.DataSources);
            if (!string.IsNullOrWhiteSpace(dataSourcesList))
            {
                var srcs = dataSourcesList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var s in srcs) project.DataSources.Add(new ProjectDataSource { SourceName = s, DisplayOrder = order++ });
            }

            // Tech
            _context.ProjectTechnologies.RemoveRange(project.Technologies);
            if (!string.IsNullOrWhiteSpace(technologiesList))
            {
                var techs = technologiesList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var t in techs) project.Technologies.Add(new ProjectTechnology { TechnologyName = t, DisplayOrder = order++ });
            }

            // Findings
            _context.ProjectKeyFindings.RemoveRange(project.KeyFindings);
            if (!string.IsNullOrWhiteSpace(findingsList))
            {
                var fnds = findingsList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var f in fnds) project.KeyFindings.Add(new ProjectKeyFinding { Finding = f, DisplayOrder = order++ });
            }

            // Domains
            _context.ProjectResearchAreas.RemoveRange(project.ProjectResearchAreas);
            if (domainIds != null)
            {
                foreach (var did in domainIds)
                {
                    project.ProjectResearchAreas.Add(new ProjectResearchArea { ResearchDomainId = did });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Project updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Project deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
