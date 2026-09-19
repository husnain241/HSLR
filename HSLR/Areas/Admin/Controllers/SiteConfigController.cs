using HSLR.Data;
using HSLR.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SiteConfigController : Controller
    {
        private readonly HsrlDbContext _context;

        public SiteConfigController(HsrlDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var config = await _context.SiteConfigs.FirstOrDefaultAsync();
            if (config == null)
            {
                config = new SiteConfig();
                _context.SiteConfigs.Add(config);
                await _context.SaveChangesAsync();
            }

            return View(config);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteConfig model)
        {
            if (!ModelState.IsValid) return View(model);

            var config = await _context.SiteConfigs.FirstOrDefaultAsync();
            if (config == null)
            {
                config = new SiteConfig();
                _context.SiteConfigs.Add(config);
            }

            config.SiteName = model.SiteName;
            config.Tagline = model.Tagline;
            config.Description = model.Description;
            config.DirectorName = model.DirectorName;
            config.ContactEmail = model.ContactEmail;
            config.Phone = model.Phone;
            config.Location = model.Location;
            config.Affiliation = model.Affiliation;
            config.TwitterUrl = model.TwitterUrl;
            config.LinkedInUrl = model.LinkedInUrl;
            config.GoogleScholarUrl = model.GoogleScholarUrl;
            config.GitHubUrl = model.GitHubUrl;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Site configuration updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
