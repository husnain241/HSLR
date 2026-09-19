using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PeopleController : Controller
    {
        private readonly HsrlDbContext _context;

        public PeopleController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var people = await _context.People
                .Include(p => p.ResearchInterests)
                .OrderBy(p => p.Category)
                .ThenBy(p => p.DisplayOrder)
                .ToListAsync();

            return View(people);
        }

        public IActionResult Create()
        {
            return View(new Person());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person, string? interestsList)
        {
            if (string.IsNullOrWhiteSpace(person.Slug))
            {
                person.Slug = person.Name.ToLowerInvariant().Replace(" ", "-").Replace(".", "");
            }

            if (await _context.People.AnyAsync(p => p.Slug == person.Slug))
            {
                ModelState.AddModelError("Slug", "A person with this slug already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(person);
            }

            if (!string.IsNullOrWhiteSpace(interestsList))
            {
                var interests = interestsList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var item in interests)
                {
                    person.ResearchInterests.Add(new PersonResearchInterest { Interest = item, DisplayOrder = order++ });
                }
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Team member '{person.Name}' added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var person = await _context.People
                .Include(p => p.ResearchInterests)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return NotFound();

            ViewBag.InterestsList = string.Join(", ", person.ResearchInterests.Select(i => i.Interest));
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person model, string? interestsList)
        {
            if (id != model.Id) return NotFound();

            var person = await _context.People
                .Include(p => p.ResearchInterests)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.InterestsList = interestsList;
                return View(model);
            }

            person.Name = model.Name;
            person.DisplayName = model.DisplayName;
            person.Slug = model.Slug;
            person.Role = model.Role;
            person.Category = model.Category;
            person.Status = model.Status;
            person.ShortBio = model.ShortBio;
            person.Biography = model.Biography;
            person.CurrentAffiliation = model.CurrentAffiliation;
            person.Featured = model.Featured;
            person.Initials = model.Initials;
            person.PhotoUrl = model.PhotoUrl;
            person.GoogleScholarUrl = model.GoogleScholarUrl;
            person.ResearchGateUrl = model.ResearchGateUrl;
            person.LinkedInUrl = model.LinkedInUrl;
            person.OrcidUrl = model.OrcidUrl;
            person.WebsiteUrl = model.WebsiteUrl;
            person.DisplayOrder = model.DisplayOrder;

            // Update interests
            _context.PersonResearchInterests.RemoveRange(person.ResearchInterests);
            if (!string.IsNullOrWhiteSpace(interestsList))
            {
                var interests = interestsList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var order = 1;
                foreach (var item in interests)
                {
                    person.ResearchInterests.Add(new PersonResearchInterest { Interest = item, DisplayOrder = order++ });
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Team member '{person.Name}' updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Team member deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
