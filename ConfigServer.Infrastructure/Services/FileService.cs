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

            
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(_uploadsFolder, fileName);

           
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

           
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string fileUrl = $"{baseUrl}/uploads/{fileName}";

            return (filePath, fileUrl);
        }

        
    
        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
