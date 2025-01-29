// using ConfigServer.Domain.Interfaces;
// using ConfigServer.Application.DTOs;
// using ConfigServer.Domain.Entities;
// using System.Text.Json;

// public class ConfigService : IConfigService
// {
//     private readonly IFileService _fileService;
//     private readonly IConfigRepository _configRepository;
//     private readonly IRabbitMQService _rabbitMQService;

//     public ConfigService(IFileService fileService, IConfigRepository configRepository, IRabbitMQService rabbitMQService)
//     {
//         _fileService = fileService;
//         _configRepository = configRepository;
//         _rabbitMQService = rabbitMQService;
//     }

//     public async Task<Config> CreateConfigAsync(ConfigDTO configDto, string userId, string userRole)
//     {
//         if (userRole != "Admin" && userRole != "Editor")
//         {
//             throw new UnauthorizedAccessException("You do not have permission to create configurations.");
//         }

//         // Handle file upload
//         string filePath = null;
//         string fileUrl = null;

//         if (configDto.File != null)
//         {
//             var result = await _fileService.UploadFileAsync(configDto.File);
//             filePath = result.filePath;
//             fileUrl = result.fileUrl;
//         }

//         var config = new Config
//         {
//             Id = Guid.NewGuid(),
//             Key = configDto.Key,
//             Value = configDto.Value,
//             Description = configDto.Description,
//             ProjectId = configDto.ProjectId,
//             CreatedAt = DateTime.UtcNow,
//             UpdatedAt = DateTime.UtcNow,
//             UserId =  userId,
//             FilePath = filePath,
//             FileUrl = fileUrl
//         };

//         await _configRepository.AddAsync(config);
//         await _configRepository.SaveChangesAsync();

//         // Send the message to RabbitMQ (if necessary)
//         var message = JsonSerializer.Serialize(new
//         {
//             ConfigId = config.Id,
//             ConfigProject = config.ProjectId,
//             ConfigFileUrl = fileUrl,
//             config.Key,
//             config.Value,
//             config.UpdatedAt
//         });

//         _rabbitMQService.PublishMessage($"config.{config.ProjectId}.{config.Key}", message);

//         return config;
//     }
// }
