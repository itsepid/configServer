using Microsoft.AspNetCore.Http;

namespace ConfigServer.Infrastructure.Services
{

public class FileService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateFileUrl(string fileName)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
        {
            throw new InvalidOperationException("Request context is unavailable.");
        }

        // Construct the base URL for the file (using scheme + host from request)
        var baseUrl = $"{request.Scheme}://{request.Host}"; 
        return $"{baseUrl}/uploads/{fileName}";
    }
}
}