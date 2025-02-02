using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
using ConfigServer.Application.Interfaces;
using ConfigServer.Infrastructure.Data;


namespace ConfigServer.Infrastructure.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly AppDbContext _dbContext;


        public ConfigRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Config>> GetAllAsync()
        {
            return await _dbContext.Configs.ToListAsync();
        }

        public async Task<Config> GetByIdAsync(Guid id)
        {
            return await _dbContext.Configs.FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            Config config = await GetByIdAsync(id);
             if (config == null)
                {
                    throw new KeyNotFoundException($"Config with ID {id} not found.");
                }
        
          _dbContext.Configs.Remove(config);

        }

        public async Task<IEnumerable<Config>> GetByProjectAsync(string projectId)
{
    if (string.IsNullOrEmpty(projectId))
    {
        throw new ArgumentException("Project cannot be null or empty.", nameof(projectId));
    }

    
    var configs = await _dbContext.Configs
        .Where(c => c.ProjectId == projectId)
        .ToListAsync();

    if (configs == null)
    {
        throw new KeyNotFoundException($"Configuration for project '{projectId}' not found.");
    }

    return configs;
}

        public async Task AddAsync(Config config)
        {
            
            await _dbContext.Configs.AddAsync(config);
        }

        public Task UpdateAsync(Config config)
        {
           
            _dbContext.Configs.Update(config);

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

            
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}