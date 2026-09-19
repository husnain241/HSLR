using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PublicationsController : Controller
    {
        private readonly HsrlDbContext _context;

        public PublicationsController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var publications = await _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .OrderByDescending(p => p.Year)
                .ThenBy(p => p.Title)
                .ToListAsync();

            return View(publications);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
            return View(new Publication { Year = DateTime.UtcNow.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publication publication, string? authorsList, int[]? domainIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
                return View(publication);
            }

            if (!string.IsNullOrWhiteSpace(authorsList))
            {
                var authors = authorsList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var author in authors)
                {
                    publication.Authors.Add(new PublicationAuthor
                    {
                        AuthorName = author,
                        IsLabMember = author.Contains("Boota") || author.Contains("Tahir") || author.Contains("Zahra") || author.Contains("Khan"),
                        DisplayOrder = order++
                    });
                }
            }

            if (domainIds != null)
            {
                foreach (var did in domainIds)
                {
                    publication.PublicationResearchAreas.Add(new PublicationResearchArea { ResearchDomainId = did });
                }
            }

            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Publication added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var publication = await _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .Include(p => p.PublicationResearchAreas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null) return NotFound();

            ViewBag.AuthorsList = string.Join(", ", publication.Authors.Select(a => a.AuthorName));
            ViewBag.SelectedDomainIds = publication.PublicationResearchAreas.Select(pra => pra.ResearchDomainId).ToList();
            ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();

            return View(publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Publication model, string? authorsList, int[]? domainIds)
        {
            if (id != model.Id) return NotFound();

            var publication = await _context.Publications
                .Include(p => p.Authors)
                .Include(p => p.PublicationResearchAreas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.AuthorsList = authorsList;
                ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
                return View(model);
            }

            publication.Title = model.Title;
            publication.Journal = model.Journal;
            publication.Year = model.Year;
            publication.Doi = model.Doi;
            publication.Url = model.Url;
            publication.PublicationType = model.PublicationType;
            publication.Abstract = model.Abstract;
            publication.Featured = model.Featured;
            publication.CitationMetrics = model.CitationMetrics;

            // Update authors
            _context.PublicationAuthors.RemoveRange(publication.Authors);
            if (!string.IsNullOrWhiteSpace(authorsList))
            {
                var authors = authorsList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var author in authors)
                {
                    publication.Authors.Add(new PublicationAuthor
                    {
                        AuthorName = author,
                        IsLabMember = author.Contains("Boota") || author.Contains("Tahir") || author.Contains("Zahra") || author.Contains("Khan"),
                        DisplayOrder = order++
                    });
                }
            }

            // Update domains
            _context.PublicationResearchAreas.RemoveRange(publication.PublicationResearchAreas);
            if (domainIds != null)
            {
                foreach (var did in domainIds)
                {
                    publication.PublicationResearchAreas.Add(new PublicationResearchArea { ResearchDomainId = did });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Publication updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publication deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
