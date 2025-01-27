using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
using ConfigServer.Domain.Interfaces;
using ConfigServer.Infrastructure.Data;


namespace ConfigServer.Infrastructure.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IRabbitMQService _rabbitMQService;

        public ConfigRepository(AppDbContext dbContext,  IRabbitMQService rabbitMQService)
        {
            _dbContext = dbContext;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<IEnumerable<Config>> GetAllAsync()
        {
            return await _dbContext.Configs.ToListAsync();
        }

        public async Task<Config> GetByIdAsync(Guid id)
        {
            return await _dbContext.Configs.FindAsync(id);
        }

        public async Task AddAsync(Config config)
        {
            
            await _dbContext.Configs.AddAsync(config);
            var message = JsonSerializer.Serialize(new
        {
            ConfigId = config.Id,
            config.Key,
            config.Value,
            config.UpdatedAt
        });

        _rabbitMQService.PublishMessage($"config.{config.Key}", message);
        }

        public Task UpdateAsync(Config config)
        {
           
            _dbContext.Configs.Update(config);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}