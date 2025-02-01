using ConfigServer.Application.Interfaces;
using ConfigServer.Application.DTOs;
using ConfigServer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConfigServer.Infrastructure.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IFileService _fileService;
        private readonly IConfigRepository _configRepository;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConfigService(
            IFileService fileService,
            IConfigRepository configRepository,
            IRabbitMQService rabbitMQService,
            IHttpContextAccessor httpContextAccessor)
        {
            _fileService = fileService;
            _configRepository = configRepository;
            _rabbitMQService = rabbitMQService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Config> CreateConfigAsync(ConfigDTO configDto, int userId, string userRole)
        {
            ValidateUserRole(userRole);

            var (filePath, fileUrl) = await HandleFileUploadAsync(configDto.File);

            var config = new Config
            {
                Id = Guid.NewGuid(),
                Key = configDto.Key,
                Value = configDto.Value,
                Description = configDto.Description,
                ProjectId = configDto.ProjectId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId,
                FilePath = filePath,
                FileUrl = fileUrl
            };

            await _configRepository.AddAsync(config);
            await _configRepository.SaveChangesAsync();

            PublishConfigMessage(config);

            return config;
        }

        public async Task UpdateConfigAsync(Guid configId, UpdateConfigDTO updatedConfig)
        {
            var existingConfig = await _configRepository.GetByIdAsync(configId);
            if (existingConfig == null)
            {
                throw new ArgumentException("Configuration not found.");
            }

            var (filePath, fileUrl) = await HandleFileReplacementAsync(existingConfig, updatedConfig.File);

            // Update the config properties
            existingConfig.Key = updatedConfig.Key;
            existingConfig.Value = updatedConfig.Value;
            existingConfig.Description = updatedConfig.Description;
            existingConfig.UpdatedAt = DateTime.UtcNow;
            existingConfig.FilePath = filePath ?? existingConfig.FilePath;
            existingConfig.FileUrl = fileUrl ?? existingConfig.FileUrl;

            await _configRepository.UpdateAsync(existingConfig);
            await _configRepository.SaveChangesAsync();

            PublishConfigMessage(existingConfig);
        }

        private void ValidateUserRole(string userRole)
        {
            if (userRole != "Admin" && userRole != "Editor")
            {
                throw new UnauthorizedAccessException("You do not have permission to create or update configurations.");
            }
        }

        private async Task<(string filePath, string fileUrl)> HandleFileUploadAsync(IFormFile file)
        {
            if (file == null)
            {
                return (null, null);
            }

            var request = _httpContextAccessor.HttpContext.Request;
            return await _fileService.UploadFileAsync(file, request);
        }

        private async Task<(string filePath, string fileUrl)> HandleFileReplacementAsync(Config existingConfig, IFormFile newFile)
        {
            if (newFile == null)
            {
                return (null, null); // No file replacement
            }

            // Delete the old file if it exists
            if (!string.IsNullOrEmpty(existingConfig.FilePath))
            {
                _fileService.DeleteFile(existingConfig.FilePath);
            }

            // Upload the new file
            var request = _httpContextAccessor.HttpContext.Request;
            return await _fileService.UploadFileAsync(newFile, request);
        }

        private void PublishConfigMessage(Config config)
        {
            var message = JsonSerializer.Serialize(new
            {
                ConfigId = config.Id,
                ConfigProject = config.ProjectId,
                ConfigFilePath = config.FilePath,
                ConfigFileUrl = config.FileUrl,
                config.Key,
                config.Value,
                config.UpdatedAt
            });

            _rabbitMQService.PublishMessage($"config.{config.ProjectId}.{config.Key}", message);
        }
    }
}
