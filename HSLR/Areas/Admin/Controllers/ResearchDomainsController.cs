using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ResearchDomainsController : Controller
    {
        private readonly HsrlDbContext _context;

        public ResearchDomainsController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var domains = await _context.ResearchDomains
                .Include(d => d.FocusTopics)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            return View(domains);
        }

        public IActionResult Create()
        {
            return View(new ResearchDomain());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResearchDomain model, string? focusTopicsList)
        {
            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                model.Slug = model.Title.ToLowerInvariant().Replace(" ", "-").Replace("&", "and");
            }

            if (await _context.ResearchDomains.AnyAsync(d => d.Slug == model.Slug))
            {
                ModelState.AddModelError("Slug", "A research domain with this slug already exists.");
            }

            if (!ModelState.IsValid) return View(model);

            if (!string.IsNullOrWhiteSpace(focusTopicsList))
            {
                var topics = focusTopicsList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var t in topics)
                {
                    model.FocusTopics.Add(new ResearchDomainFocusTopic { Topic = t, DisplayOrder = order++ });
                }
            }

            _context.ResearchDomains.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Research domain created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var domain = await _context.ResearchDomains
                .Include(d => d.FocusTopics.OrderBy(t => t.DisplayOrder))
                .FirstOrDefaultAsync(d => d.Id == id);

            if (domain == null) return NotFound();

            ViewBag.FocusTopicsList = string.Join("\n", domain.FocusTopics.Select(t => t.Topic));
            return View(domain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResearchDomain model, string? focusTopicsList)
        {
            if (id != model.Id) return NotFound();

            var domain = await _context.ResearchDomains
                .Include(d => d.FocusTopics)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (domain == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.FocusTopicsList = focusTopicsList;
                return View(model);
            }

            domain.Title = model.Title;
            domain.Slug = model.Slug;
            domain.ShortDescription = model.ShortDescription;
            domain.Description = model.Description;
            domain.IconName = model.IconName;
            domain.Featured = model.Featured;
            domain.DisplayOrder = model.DisplayOrder;

            // Update topics
            _context.ResearchDomainFocusTopics.RemoveRange(domain.FocusTopics);
            if (!string.IsNullOrWhiteSpace(focusTopicsList))
            {
                var topics = focusTopicsList.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var t in topics)
                {
                    domain.FocusTopics.Add(new ResearchDomainFocusTopic { Topic = t, DisplayOrder = order++ });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Research domain updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var domain = await _context.ResearchDomains.FindAsync(id);
            if (domain != null)
            {
                _context.ResearchDomains.Remove(domain);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Research domain removed.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
