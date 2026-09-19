using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventsController : Controller
    {
        private readonly HsrlDbContext _context;

        public EventsController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        public IActionResult Create()
        {
            return View(new Event
            {
                EventDate = DateTime.UtcNow.AddDays(7),
                Status = EventStatus.Upcoming
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                model.Slug = model.Title.ToLowerInvariant().Replace(" ", "-").Replace(":", "").Replace(".", "");
            }

            if (await _context.Events.AnyAsync(e => e.Slug == model.Slug))
            {
                ModelState.AddModelError("Slug", "An event with this slug already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Events.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event scheduled successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            ev.Title = model.Title;
            ev.Slug = model.Slug;
            ev.EventType = model.EventType;
            ev.EventDate = model.EventDate;
            ev.EndDate = model.EndDate;
            ev.Location = model.Location;
            ev.Description = model.Description;
            ev.Speaker = model.Speaker;
            ev.Affiliation = model.Affiliation;
            ev.Status = model.Status;
            ev.Featured = model.Featured;
            ev.LinkUrl = model.LinkUrl;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event deleted.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
