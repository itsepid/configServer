using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ConfigServer.Application.DTOs{
public class UpdateConfigDTO
{
    [Required(ErrorMessage = "Key is required.")]
    [MaxLength(100, ErrorMessage = "Key cannot exceed 100 characters.")]
    public string Key { get; set; }

    [Required(ErrorMessage = "Value is required.")]
    [MaxLength(500, ErrorMessage = "Value cannot exceed 500 characters.")]
    public string Value { get; set; }

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; }

    public IFormFile File { get; set; }
}
}