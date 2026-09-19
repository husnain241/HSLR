using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InquiriesController : Controller
    {
        private readonly HsrlDbContext _context;

        public InquiriesController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(InquiryStatus? status, bool showSpam = false)
        {
            var query = _context.CollaborationInquiries
                .Include(i => i.ResearchDomain)
                .AsQueryable();

            if (!showSpam)
            {
                query = query.Where(i => !i.IsSpam);
            }

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            var inquiries = await query
                .OrderByDescending(i => i.SubmittedAtUtc)
                .ToListAsync();

            ViewBag.SelectedStatus = status;
            ViewBag.ShowSpam = showSpam;
            ViewBag.NewCount = await _context.CollaborationInquiries.CountAsync(i => i.Status == InquiryStatus.New && !i.IsSpam);

            return View(inquiries);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inquiry = await _context.CollaborationInquiries
                .Include(i => i.ResearchDomain)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inquiry == null) return NotFound();

            return View(inquiry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, InquiryStatus status, string? adminNotes)
        {
            var inquiry = await _context.CollaborationInquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            inquiry.Status = status;
            inquiry.AdminNotes = adminNotes;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Inquiry #{inquiry.Id} status updated to {status}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSpam(int id)
        {
            var inquiry = await _context.CollaborationInquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            inquiry.IsSpam = !inquiry.IsSpam;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Inquiry #{inquiry.Id} spam flag set to {inquiry.IsSpam}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var inquiry = await _context.CollaborationInquiries.FindAsync(id);
            if (inquiry != null)
            {
                _context.CollaborationInquiries.Remove(inquiry);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Inquiry #{id} removed from database.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
