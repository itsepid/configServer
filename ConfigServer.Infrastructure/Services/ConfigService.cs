// using ConfigServer.Application.Interfaces;
// using ConfigServer.Application.DTOs;
// using ConfigServer.Domain.Entities;
// using Microsoft.AspNetCore.Http;
// using System;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace ConfigServer.Infrastructure.Services
// {
//     public class ConfigService : IConfigService
//     {
//         private readonly IFileService _fileService;
//         private readonly IConfigRepository _configRepository;
//         private readonly IRabbitMQService _rabbitMQService;
//         private readonly IHttpContextAccessor _httpContextAccessor;

//         public ConfigService(
//             IFileService fileService,
//             IConfigRepository configRepository,
//             IRabbitMQService rabbitMQService,
//             IHttpContextAccessor httpContextAccessor)
//         {
//             _fileService = fileService;
//             _configRepository = configRepository;
//             _rabbitMQService = rabbitMQService;
//             _httpContextAccessor = httpContextAccessor;
//         }

//         public async Task<Config> CreateConfigAsync(ConfigDTO configDto, int userId, string userRole)
//         {
//             ValidateUserRole(userRole);

//             var (filePath, fileUrl) = await HandleFileUploadAsync(configDto.File);

//             var config = new Config
//             {
//                 Id = Guid.NewGuid(),
//                 Key = configDto.Key,
//                 Value = configDto.Value,
//                 Description = configDto.Description,
//                 ProjectId = configDto.ProjectId,
//                 CreatedAt = DateTime.UtcNow,
//                 UpdatedAt = DateTime.UtcNow,
//                 UserId = userId,
//                 FilePath = filePath,
//                 FileUrl = fileUrl
//             };

//             await _configRepository.AddAsync(config);
//             await _configRepository.SaveChangesAsync();

//             PublishConfigMessage(config, "create");

//             return config;
        
//         }

//         public async Task DeleteConfigAsync(Guid id, string userRole)
//         {
//            ValidateUserRole(userRole);

//            var config = await _configRepository.GetByIdAsync(id);

//            if (config == null)
//             {
//                 throw new ArgumentException($"Configuration with ID {id} not found.");
//             }

//            await _configRepository.DeleteAsync(id);
//            await _configRepository.SaveChangesAsync();
//            PublishDeleteMessage(config);
//         }
// public async Task UpdateConfigAsync(Guid configId, UpdateConfigDTO updatedConfig, string userRole)
// {
//     if (updatedConfig == null)
//     {
//         throw new ArgumentNullException(nameof(updatedConfig));
//     }

//     Console.WriteLine($"Updating config with ID: {configId}");
//     Console.WriteLine($"Key: {updatedConfig.Key}, Value: {updatedConfig.Value}, Description: {updatedConfig.Description}");
//     Console.WriteLine($"File provided: {updatedConfig.File != null}");

//     ValidateUserRole(userRole);

//     var existingConfig = await _configRepository.GetByIdAsync(configId);
//     if (existingConfig == null)
//     {
//         throw new ArgumentException($"Configuration with ID {configId} not found.");
//     }

//     string filePath = existingConfig.FilePath;
//     string fileUrl = existingConfig.FileUrl;

//     try
//     {
        
//         if (updatedConfig.File != null)
//         {
//             if (updatedConfig.File.Length == 0)
//             {
//                 throw new ArgumentException("Invalid file provided.");
//             }

//             var (newFilePath, newFileUrl) = await HandleFileReplacementAsync(existingConfig, updatedConfig.File);
//             filePath = newFilePath ?? filePath;
//             fileUrl = newFileUrl ?? fileUrl;
//         }

     
//         existingConfig.Key = updatedConfig.Key ?? existingConfig.Key;
//         existingConfig.Value = updatedConfig.Value ?? existingConfig.Value;
//         existingConfig.Description = updatedConfig.Description ?? existingConfig.Description;
//         existingConfig.UpdatedAt = DateTime.UtcNow;
//         existingConfig.FilePath = filePath;
//         existingConfig.FileUrl = fileUrl;
//         Console.WriteLine($"UpdatedAt before save: {existingConfig.UpdatedAt}");


//         await _configRepository.UpdateAsync(existingConfig);

//         await _configRepository.SaveChangesAsync();
//         Console.WriteLine($"UpdatedAt after save: {existingConfig.UpdatedAt}");

//         PublishConfigMessage(existingConfig, "Update");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error updating config: {ex.Message}");
//         throw; 
//     }
// }

// private async Task<(string filePath, string fileUrl)> HandleFileReplacementAsync(Config existingConfig, IFormFile newFile)
// {
//     if (newFile == null)
//     {
//         return (null, null); 
//     }

//     // Upload the new file first
//     var request = _httpContextAccessor.HttpContext.Request;
//     var (newFilePath, newFileUrl) = await _fileService.UploadFileAsync(newFile, request);

//     // Delete the old file if it exists
//     if (!string.IsNullOrEmpty(existingConfig.FilePath))
//     {
//         _fileService.DeleteFile(existingConfig.FilePath);
//     }

//     return (newFilePath, newFileUrl);


// }


        

//         private void ValidateUserRole(string userRole)
//         {
//             if (userRole != "Admin" && userRole != "Editor")
//             {
//                 throw new UnauthorizedAccessException("You do not have permission to create or update configurations.");
//             }
//         }

//         private async Task<(string filePath, string fileUrl)> HandleFileUploadAsync(IFormFile file)
//         {
//             if (file == null)
//             {
//                 return (null, null);
//             }

//             var request = _httpContextAccessor.HttpContext.Request;
//             return await _fileService.UploadFileAsync(file, request);
//         }


//         private void PublishConfigMessage(Config config, string action)
//         {
//             var message =  (new
//             {
//                 Action = action,
//                 ConfigId = config.Id,
//                 ConfigProject = config.ProjectId,
//                 ConfigFilePath = config.FilePath,
//                 ConfigFileUrl = config.FileUrl,
//                 config.Key,
//                 config.Value,
//                 config.UpdatedAt
//             });

//                 var messageBody = JsonSerializer.Serialize(message);

//             _rabbitMQService.PublishMessage($"config.{config.ProjectId}.{config.Key}", messageBody);
//         }

//         private void PublishDeleteMessage(Config config)
//         {

//             var message = JsonSerializer.Serialize (new
//             {
//                 ConfigId = config.Id,
//                 Action = "Deleted"
//             });

//              _rabbitMQService.PublishMessage($"config.{config.ProjectId}.{config.Key}", message);

//         }
//     }
// }
