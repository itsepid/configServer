using ConfigServer.Application.Interfaces;
using ConfigServer.Domain.Entities;
using ConfigServer.Infrastructure.Data;
using ConfigServer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigServer.Infrastructure.Repositories
{
    public class ConfRepository : IConfRepository
    {
        private readonly AppDbContext _context;
        private readonly IRabbitMQService _rabbitMQService;

        public ConfRepository(AppDbContext context, IRabbitMQService rabbitMQService)
        {
            _context = context;
             _rabbitMQService = rabbitMQService;
        }

        public async Task<ConfigProject> GetProjectByNameAsync(string projectName)
        {
            return await _context.ConfigProjects
                .FirstOrDefaultAsync(p => p.ProjectName == projectName);
        }

        public async Task<IEnumerable<ConfigEntry>> GetProjectConfigAsync(Guid projectId, string environment)
        {
            var configEntries = await _context.ConfigEntries
                .Where(e => e.ConfigProjectId == projectId && e.Environment == environment)
                .ToListAsync();

            return configEntries;
        }

 public async Task UpdateProjectConfigAsync(Guid projectId, Dictionary<string, string> newConfig, string environment)
{
    var existingConfigs = await _context.ConfigEntries
        .Where(e => e.ConfigProjectId == projectId)
        .ToListAsync();

    
    var environments = existingConfigs
        .Select(e => e.Environment)
        .Where(env => env != "*")
        .Distinct()
        .ToList();

    foreach (var item in newConfig)
    {
        if (environment == "*")
        {
            
            foreach (var env in environments)
            {
                var existingEntry = existingConfigs.FirstOrDefault(e => e.Key == item.Key && e.Environment == env);
                
                if (existingEntry != null)
                {
                    
                    existingEntry.UpdateValue(item.Value);
                }
                else
                {
                   
                    _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value, env));
                }
                await _rabbitMQService.PublishConfigUpdateAsync(projectId, env, newConfig);
            }

           
            var wildcardEntry = existingConfigs.FirstOrDefault(e => e.Key == item.Key && e.Environment == "*");
            if (wildcardEntry != null)
            {
                wildcardEntry.UpdateValue(item.Value);
            }
            else
            {
                _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value, "*"));
            }
        }
        else
        {
         
            var configEntry = existingConfigs.FirstOrDefault(e => e.Key == item.Key && e.Environment == environment);

            if (configEntry != null)
            {
                configEntry.UpdateValue(item.Value);
            }
            else
            {
                _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value, environment));
            }
            await _rabbitMQService.PublishConfigUpdateAsync(projectId, environment, newConfig);
        }
    }

    await _context.SaveChangesAsync();
}



        public async Task UpdateConfigAsync(Guid projectId, Dictionary<string, string> newConfig, string environment)
        {
            foreach (var item in newConfig)
            {
                var configEntry = await _context.ConfigEntries
                    .FirstOrDefaultAsync(e => e.ConfigProjectId == projectId && e.Environment == environment && e.Key == item.Key);

                if (configEntry != null)
                {
                    configEntry.UpdateValue(item.Value); 
                }
                else
                {
                    _context.ConfigEntries.Add(new ConfigEntry(projectId, item.Key, item.Value, environment)); 
                }
            }

            await _context.SaveChangesAsync();
        }


         public async Task CreateProjectAsync(ConfigProject project)
        {
            _context.ConfigProjects.Add(project);
            await _context.SaveChangesAsync();
        }
    }
}
