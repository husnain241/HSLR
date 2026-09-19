using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class EventsController : Controller
    {
        private readonly HsrlDbContext _context;

        public EventsController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("events")]
        public async Task<IActionResult> Index()
        {
            var upcoming = await _context.Events
                .Where(e => e.Status == EventStatus.Upcoming)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            var past = await _context.Events
                .Where(e => e.Status == EventStatus.Past)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            ViewBag.UpcomingEvents = upcoming;
            ViewBag.PastEvents = past;

            return View();
        }
    }
}
