using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class GalleryController : Controller
    {
        private readonly HsrlDbContext _context;

        public GalleryController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("gallery")]
        public async Task<IActionResult> Index(GalleryCategory? category)
        {
            var query = _context.GalleryItems
                .Include(g => g.Event)
                .Include(g => g.GalleryResearchAreas)
                    .ThenInclude(gra => gra.ResearchDomain)
                .AsQueryable();

            if (category.HasValue)
            {
                query = query.Where(g => g.Category == category.Value);
            }

            var items = await query
                .OrderBy(g => g.DisplayOrder)
                .ThenByDescending(g => g.EventDate)
                .ToListAsync();

            ViewBag.SelectedCategory = category;
            return View(items);
        }
    }
}
