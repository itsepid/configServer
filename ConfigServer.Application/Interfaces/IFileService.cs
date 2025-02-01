using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ConfigServer.Application.Interfaces
{
    public interface IFileService
    {
     
        Task<(string filePath, string fileUrl)> UploadFileAsync(IFormFile file, HttpRequest request);
        public void DeleteFile(string filePath);

        
    }
}
