using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
using ConfigServer.Domain.Interfaces;
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

        public async Task AddAsync(Config config)
        {
            
            await _dbContext.Configs.AddAsync(config);
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