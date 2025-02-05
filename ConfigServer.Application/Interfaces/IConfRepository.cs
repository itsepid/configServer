using ConfigServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigServer.Application.Interfaces
{
    public interface IConfRepository
    {
        Task<ConfigProject> GetProjectByNameAsync(string projectName);
        Task<IEnumerable<ConfigEntry>> GetProjectConfigAsync(Guid projectId, string environment);
        Task UpdateProjectConfigAsync(Guid projectId, Dictionary<string, string> newConfig, string environment);
        Task CreateProjectAsync(ConfigProject project);
      
      Task UpdateConfigAsync(Guid projectId, Dictionary<string, string> newConfig, string environment);
    }
}
