using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class OpportunitiesController : Controller
    {
        private readonly HsrlDbContext _context;

        public OpportunitiesController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("opportunities")]
        public async Task<IActionResult> Index()
        {
            var openPositions = await _context.OfficialOpportunities
                .OrderBy(o => o.DisplayOrder)
                .ThenBy(o => o.Deadline)
                .ToListAsync();

            var pathways = await _context.OpportunityCategories
                .Include(c => c.RecurringPathways.OrderBy(p => p.DisplayOrder))
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            ViewBag.OpenPositions = openPositions;
            ViewBag.Pathways = pathways;

            return View();
        }
    }
}
