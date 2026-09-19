using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OpportunitiesController : Controller
    {
        private readonly HsrlDbContext _context;

        public OpportunitiesController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var opps = await _context.OfficialOpportunities
                .OrderBy(o => o.DisplayOrder)
                .ThenBy(o => o.Deadline)
                .ToListAsync();

            return View(opps);
        }

        public IActionResult Create()
        {
            return View(new OfficialOpportunity
            {
                Deadline = DateTime.UtcNow.AddDays(30),
                Status = OpportunityStatus.Open
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OfficialOpportunity model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.OfficialOpportunities.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Opportunity posted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var opp = await _context.OfficialOpportunities.FindAsync(id);
            if (opp == null) return NotFound();
            return View(opp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OfficialOpportunity model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var opp = await _context.OfficialOpportunities.FindAsync(id);
            if (opp == null) return NotFound();

            opp.Title = model.Title;
            opp.Category = model.Category;
            opp.Deadline = model.Deadline;
            opp.Description = model.Description;
            opp.Status = model.Status;
            opp.Link = model.Link;
            opp.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Opportunity updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var opp = await _context.OfficialOpportunities.FindAsync(id);
            if (opp != null)
            {
                _context.OfficialOpportunities.Remove(opp);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Opportunity removed.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
