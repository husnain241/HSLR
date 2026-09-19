using HSLR.Data;
using HSLR.Models.Entities;
using HSLR.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly HsrlDbContext _context;
        private const int PageSize = 5;

        public PublicationsController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("publications")]
        public async Task<IActionResult> Index(string? search, int? year, PublicationType? type, int? domainId, int page = 1)
        {
            if (page < 1) page = 1;

            var query = _context.Publications
                .Include(p => p.Authors.OrderBy(a => a.DisplayOrder))
                .Include(p => p.PublicationResearchAreas)
                    .ThenInclude(pra => pra.ResearchDomain)
                .AsQueryable();

            // 1. Text Search Filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                query = query.Where(p =>
                    p.Title.Contains(term) ||
                    (p.Journal != null && p.Journal.Contains(term)) ||
                    p.Authors.Any(a => a.AuthorName.Contains(term)) ||
                    (p.Abstract != null && p.Abstract.Contains(term))
                );
            }

            // 2. Year Filter
            if (year.HasValue)
            {
                query = query.Where(p => p.Year == year.Value);
            }

            // 3. Type Filter
            if (type.HasValue)
            {
                query = query.Where(p => p.PublicationType == type.Value);
            }

            // 4. Domain Filter
            if (domainId.HasValue)
            {
                query = query.Where(p => p.PublicationResearchAreas.Any(pra => pra.ResearchDomainId == domainId.Value));
            }

            var totalItems = await query.CountAsync();

            var publications = await query
                .OrderByDescending(p => p.Year)
                .ThenBy(p => p.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var availableYears = await _context.Publications
                .Select(p => p.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            var availableDomains = await _context.ResearchDomains
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            var model = new PublicationsIndexViewModel
            {
                Search = search,
                Year = year,
                Type = type,
                DomainId = domainId,
                Page = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Publications = publications,
                AvailableYears = availableYears,
                AvailableDomains = availableDomains
            };

            return View(model);
        }
    }
}
