using ConfigServer.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConfigServer.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _uploadsFolder;

        public FileService()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }
        public async Task<(string filePath, string fileUrl)> UploadFileAsync(IFormFile file, HttpRequest request)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            // Generate unique file name
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Construct base URL from the request
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string fileUrl = $"{baseUrl}/uploads/{fileName}";

            return (filePath, fileUrl);
        }

        /// <summary>
        /// Deletes a file from the uploads folder.
        /// </summary>
        /// <param name="filePath">The path of the file to delete.</param>
        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
