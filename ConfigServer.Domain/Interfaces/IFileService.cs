using Microsoft.AspNetCore.Http;
namespace ConfigServer.Domain.Interfaces{
public interface IFileService
{
    Task<(string filePath, string fileUrl)> UploadFileAsync(IFormFile file);
}
}
