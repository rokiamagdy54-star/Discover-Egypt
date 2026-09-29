using Tourism.Core.Exceptions;
using Tourism.Core.Features.UploadImage.Interfaces;

namespace Tourism.API.Extensions
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ValidationException("Invalid file");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(file.ContentType))
                throw new ValidationException("Only images are allowed (jpeg, png, jpg)");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/{folderName}/{fileName}";
        }
    }
}