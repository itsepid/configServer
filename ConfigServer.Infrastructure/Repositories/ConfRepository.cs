using ConfigServer.Application.Interfaces;
using ConfigServer.Domain.Entities;
using ConfigServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigServer.Infrastructure.Repositories
{
    public class ConfRepository : IConfRepository
    {
        private readonly AppDbContext _context;

        public ConfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConfigProject> GetProjectByNameAsync(string projectName)
        {
            return await _context.ConfigProjects
                .FirstOrDefaultAsync(p => p.ProjectName == projectName);
        }

        public async Task<IEnumerable<ConfigEntry>> GetProjectConfigAsync(Guid projectId)
        {
            var configEntries = await _context.ConfigEntries
                .Where(e => e.ConfigProjectId == projectId)
                .ToListAsync();

            return configEntries;
        }

        public async Task UpdateProjectConfigAsync(Guid projectId, Dictionary<string, string> newConfig)
        {
            foreach (var item in newConfig)
            {
                var configEntry = await _context.ConfigEntries
                    .FirstOrDefaultAsync(e => e.ConfigProjectId == projectId && e.Key == item.Key);

                if (configEntry != null)
                {
                    configEntry.UpdateValue(item.Value);
                }
                else
                {
                    _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value));
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task UpdateConfigAsync(Guid projectId, Dictionary<string, string> newConfig)
        {
            foreach (var item in newConfig)
            {
                var configEntry = await _context.ConfigEntries
                    .FirstOrDefaultAsync(e => e.ConfigProjectId == projectId && e.Key == item.Key);

                if (configEntry != null)
                {
                    configEntry.UpdateValue(item.Value); // Update existing entry
                }
                else
                {
                    _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value)); // Add new entry
                }
            }

            await _context.SaveChangesAsync();
        }


    //         public async Task UpdateConfigAsync(Guid projectId, string key, string value)
    // {
    //     var config = await _context.ConfigEntries
    //         .FirstOrDefaultAsync(c => c.ConfigProjectId == projectId && c.Key == key);

    //     if (config == null)
    //     {
    //         throw new KeyNotFoundException($"Configuration '{key}' not found for project '{projectId}'.");
    //     }

    //     config.Value = value;
    //   //  config.LastUpdated = DateTime.UtcNow;

    //     await _context.SaveChangesAsync();
    // }


         public async Task CreateProjectAsync(ConfigProject project)
        {
            _context.ConfigProjects.Add(project);
            await _context.SaveChangesAsync();
        }
    }
}
