using HSLR.Data;
using HSLR.Models.Entities;
using HSLR.Services;
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
        private readonly IImageStorageService _imageStorage;

        public PublicationsController(HsrlDbContext context, IImageStorageService imageStorage)
        {
            _context = context;
            _imageStorage = imageStorage;
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
        public async Task<IActionResult> Create(
            Publication publication,
            IFormFile? imageFile1,
            IFormFile? imageFile2,
            IFormFile? imageFile3,
            IFormFile? imageFile4,
            string? authorsList,
            int[]? domainIds)
        {
            if (imageFile1 != null && imageFile1.Length > 0)
            {
                try { publication.Image1Url = await _imageStorage.SaveImageAsync(imageFile1, "publications"); }
                catch (Exception ex) { ModelState.AddModelError("Image1Url", ex.Message); }
            }
            if (imageFile2 != null && imageFile2.Length > 0)
            {
                try { publication.Image2Url = await _imageStorage.SaveImageAsync(imageFile2, "publications"); }
                catch (Exception ex) { ModelState.AddModelError("Image2Url", ex.Message); }
            }
            if (imageFile3 != null && imageFile3.Length > 0)
            {
                try { publication.Image3Url = await _imageStorage.SaveImageAsync(imageFile3, "publications"); }
                catch (Exception ex) { ModelState.AddModelError("Image3Url", ex.Message); }
            }
            if (imageFile4 != null && imageFile4.Length > 0)
            {
                try { publication.Image4Url = await _imageStorage.SaveImageAsync(imageFile4, "publications"); }
                catch (Exception ex) { ModelState.AddModelError("Image4Url", ex.Message); }
            }

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
        public async Task<IActionResult> Edit(
            int id,
            Publication model,
            IFormFile? imageFile1,
            IFormFile? imageFile2,
            IFormFile? imageFile3,
            IFormFile? imageFile4,
            string? authorsList,
            int[]? domainIds)
        {
            if (id != model.Id) return NotFound();

            var publication = await _context.Publications
                .Include(p => p.Authors)
                .Include(p => p.PublicationResearchAreas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null) return NotFound();

            // Image 1
            if (imageFile1 != null && imageFile1.Length > 0)
            {
                try
                {
                    var newUrl = await _imageStorage.SaveImageAsync(imageFile1, "publications");
                    if (!string.IsNullOrWhiteSpace(publication.Image1Url) && publication.Image1Url.StartsWith("/images/publications/"))
                    {
                        _imageStorage.DeleteImage(publication.Image1Url);
                    }
                    publication.Image1Url = newUrl;
                }
                catch (Exception ex) { ModelState.AddModelError("Image1Url", ex.Message); }
            }
            else
            {
                publication.Image1Url = model.Image1Url;
            }

            // Image 2
            if (imageFile2 != null && imageFile2.Length > 0)
            {
                try
                {
                    var newUrl = await _imageStorage.SaveImageAsync(imageFile2, "publications");
                    if (!string.IsNullOrWhiteSpace(publication.Image2Url) && publication.Image2Url.StartsWith("/images/publications/"))
                    {
                        _imageStorage.DeleteImage(publication.Image2Url);
                    }
                    publication.Image2Url = newUrl;
                }
                catch (Exception ex) { ModelState.AddModelError("Image2Url", ex.Message); }
            }
            else
            {
                publication.Image2Url = model.Image2Url;
            }

            // Image 3
            if (imageFile3 != null && imageFile3.Length > 0)
            {
                try
                {
                    var newUrl = await _imageStorage.SaveImageAsync(imageFile3, "publications");
                    if (!string.IsNullOrWhiteSpace(publication.Image3Url) && publication.Image3Url.StartsWith("/images/publications/"))
                    {
                        _imageStorage.DeleteImage(publication.Image3Url);
                    }
                    publication.Image3Url = newUrl;
                }
                catch (Exception ex) { ModelState.AddModelError("Image3Url", ex.Message); }
            }
            else
            {
                publication.Image3Url = model.Image3Url;
            }

            // Image 4
            if (imageFile4 != null && imageFile4.Length > 0)
            {
                try
                {
                    var newUrl = await _imageStorage.SaveImageAsync(imageFile4, "publications");
                    if (!string.IsNullOrWhiteSpace(publication.Image4Url) && publication.Image4Url.StartsWith("/images/publications/"))
                    {
                        _imageStorage.DeleteImage(publication.Image4Url);
                    }
                    publication.Image4Url = newUrl;
                }
                catch (Exception ex) { ModelState.AddModelError("Image4Url", ex.Message); }
            }
            else
            {
                publication.Image4Url = model.Image4Url;
            }

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
                string?[] images = [publication.Image1Url, publication.Image2Url, publication.Image3Url, publication.Image4Url];
                foreach (var img in images)
                {
                    if (!string.IsNullOrWhiteSpace(img) && img.StartsWith("/images/publications/"))
                    {
                        _imageStorage.DeleteImage(img);
                    }
                }

                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publication deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
