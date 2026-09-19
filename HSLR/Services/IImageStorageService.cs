namespace HSLR.Services
{
    public interface IImageStorageService
    {
        Task<string> SaveImageAsync(IFormFile file, string category);
        void DeleteImage(string relativeUrl);
    }
}
