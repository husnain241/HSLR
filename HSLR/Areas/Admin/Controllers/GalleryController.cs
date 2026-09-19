using HSLR.Data;
using HSLR.Models.Entities;
using HSLR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GalleryController : Controller
    {
        private readonly HsrlDbContext _context;
        private readonly IImageStorageService _imageStorage;

        public GalleryController(HsrlDbContext context, IImageStorageService imageStorage)
        {
            _context = context;
            _imageStorage = imageStorage;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.GalleryItems
                .OrderBy(g => g.Category)
                .ThenBy(g => g.DisplayOrder)
                .ToListAsync();

            return View(items);
        }

        public IActionResult Create()
        {
            return View(new GalleryItem { EventDate = DateTime.UtcNow });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GalleryItem model, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    model.ImageUrl = await _imageStorage.SaveImageAsync(imageFile, "gallery");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            if (string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                ModelState.AddModelError("ImageUrl", "Please upload an image file or specify an image URL.");
            }

            if (!ModelState.IsValid) return View(model);

            _context.GalleryItems.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gallery item added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.GalleryItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GalleryItem model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();

            var item = await _context.GalleryItems.FindAsync(id);
            if (item == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    var newUrl = await _imageStorage.SaveImageAsync(imageFile, "gallery");
                    _imageStorage.DeleteImage(item.ImageUrl);
                    item.ImageUrl = newUrl;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            if (!ModelState.IsValid) return View(model);

            item.Title = model.Title;
            item.AltText = model.AltText;
            item.Caption = model.Caption;
            item.Category = model.Category;
            item.EventDate = model.EventDate;
            item.Location = model.Location;
            item.DisplayOrder = model.DisplayOrder;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Gallery item updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.GalleryItems.FindAsync(id);
            if (item != null)
            {
                _imageStorage.DeleteImage(item.ImageUrl);
                _context.GalleryItems.Remove(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gallery item deleted.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
