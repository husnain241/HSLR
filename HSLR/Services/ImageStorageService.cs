namespace HSLR.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageStorageService> _logger;
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".svg"];

        public ImageStorageService(IWebHostEnvironment environment, ILogger<ImageStorageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string category)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Uploaded file cannot be empty.", nameof(file));
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Unsupported file format '{extension}'. Allowed formats: JPG, PNG, WEBP, SVG.");
            }

            var safeCategory = string.Join("_", category.Split(Path.GetInvalidFileNameChars())).ToLowerInvariant();
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", safeCategory);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("Saved image to {FullPath}", fullPath);
            return $"/images/{safeCategory}/{uniqueFileName}";
        }

        public void DeleteImage(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl)) return;

            try
            {
                var cleanRelative = relativeUrl.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(_environment.WebRootPath, cleanRelative);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("Deleted image at {FullPath}", fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete image at {Url}", relativeUrl);
            }
        }
    }
}
