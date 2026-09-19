using HSLR.Data;
using HSLR.Models.Entities;
using HSLR.Models.ViewModels;
using HSLR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Controllers
{
    public class CollaborationController : Controller
    {
        private readonly HsrlDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<CollaborationController> _logger;

        public CollaborationController(
            HsrlDbContext context,
            IEmailService emailService,
            ILogger<CollaborationController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet("collaborate")]
        [HttpGet("contact")]
        public async Task<IActionResult> Index()
        {
            var domains = await _context.ResearchDomains
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            var sectors = await _context.CollaborationSectors
                .Include(s => s.EngagementAvenues.OrderBy(a => a.DisplayOrder))
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            var config = await _context.SiteConfigs.FirstOrDefaultAsync() ?? new SiteConfig();

            ViewBag.Domains = domains;
            ViewBag.Sectors = sectors;
            ViewBag.Config = config;

            return View(new CollaborationInquiryViewModel());
        }

        [HttpPost("collaborate/submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(CollaborationInquiryViewModel model)
        {
            // 1. Basic Spam Detection (Honeypot trap)
            var isSpam = !string.IsNullOrWhiteSpace(model.WebsiteTrap);
            if (isSpam)
            {
                _logger.LogWarning("Spam bot trap triggered for email {Email}", model.Email);
            }

            // 2. Server-side validation
            if (!ModelState.IsValid)
            {
                ViewBag.Domains = await _context.ResearchDomains.OrderBy(d => d.DisplayOrder).ToListAsync();
                ViewBag.Sectors = await _context.CollaborationSectors
                    .Include(s => s.EngagementAvenues.OrderBy(a => a.DisplayOrder))
                    .OrderBy(s => s.DisplayOrder)
                    .ToListAsync();
                ViewBag.Config = await _context.SiteConfigs.FirstOrDefaultAsync() ?? new SiteConfig();
                return View("Index", model);
            }

            // 3. Save submission as CollaborationInquiry row with Status = New
            var inquiry = new CollaborationInquiry
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Organization = model.Organization.Trim(),
                AffiliationType = model.AffiliationType,
                CollaborationType = model.CollaborationType,
                ResearchDomainId = model.ResearchDomainId,
                Title = model.Title?.Trim(),
                Description = model.Description.Trim(),
                SubmittedAtUtc = DateTime.UtcNow,
                Status = InquiryStatus.New,
                IsSpam = isSpam
            };

            _context.CollaborationInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            var config = await _context.SiteConfigs.FirstOrDefaultAsync() ?? new SiteConfig();

            // 4. Send notification email and confirmation email (if not spam)
            if (!isSpam)
            {
                try
                {
                    await _emailService.SendInquiryNotificationAsync(inquiry, config);
                    await _emailService.SendInquiryConfirmationAsync(inquiry, config);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send inquiry notification emails for #{InquiryId}", inquiry.Id);
                    // Do not fail submission page if SMTP server is unavailable
                }
            }

            // 5. Redirect to real confirmation page
            return RedirectToAction(nameof(Confirmation), new { id = inquiry.Id });
        }

        [HttpGet("collaborate/confirmation/{id}")]
        public async Task<IActionResult> Confirmation(int id)
        {
            var inquiry = await _context.CollaborationInquiries
                .Include(i => i.ResearchDomain)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inquiry == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var config = await _context.SiteConfigs.FirstOrDefaultAsync() ?? new SiteConfig();
            ViewBag.Config = config;

            return View(inquiry);
        }
    }
}
