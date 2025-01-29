using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigServer.Domain.Entities;

namespace ConfigServer.Domain.Interfaces 
{
    public interface IConfigRepository
    {
        Task<IEnumerable<Config>> GetAllAsync();
        Task<Config> GetByIdAsync(Guid id);
        Task<IEnumerable<Config>> GetByProjectAsync(string project);
        Task AddAsync (Config config);
        Task UpdateAsync (Config config);
        Task SaveChangesAsync();

    }
}