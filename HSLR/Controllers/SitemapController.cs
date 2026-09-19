using HSLR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HSLR.Controllers
{
    public class SitemapController : Controller
    {
        private readonly HsrlDbContext _context;

        public SitemapController(HsrlDbContext context)
        {
            _context = context;
        }

        [Route("sitemap.xml")]
        [ResponseCache(Duration = 3600)]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var projects = await _context.Projects
                .Select(p => p.Slug)
                .ToListAsync();

            var xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xml.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            // Static public routes
            var staticRoutes = new[]
            {
                "",
                "/professor",
                "/research-family",
                "/research",
                "/publications",
                "/projects",
                "/events",
                "/gallery",
                "/opportunities",
                "/collaborate",
                "/contact"
            };

            foreach (var route in staticRoutes)
            {
                xml.AppendLine("  <url>");
                xml.AppendLine($"    <loc>{baseUrl}{route}</loc>");
                xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                xml.AppendLine("    <changefreq>weekly</changefreq>");
                xml.AppendLine($"    <priority>{(route == "" ? "1.0" : "0.8")}</priority>");
                xml.AppendLine("  </url>");
            }

            // Project detail pages
            foreach (var slug in projects)
            {
                xml.AppendLine("  <url>");
                xml.AppendLine($"    <loc>{baseUrl}/projects/{slug}</loc>");
                xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
                xml.AppendLine("    <changefreq>monthly</changefreq>");
                xml.AppendLine("    <priority>0.7</priority>");
                xml.AppendLine("  </url>");
            }

            xml.AppendLine("</urlset>");

            return Content(xml.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
