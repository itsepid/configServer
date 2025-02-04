using ConfigServer.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigServer.Application.Interfaces
{
    public interface IConfRepository
    {
        Task<ConfigProject> GetProjectByNameAsync(string projectName);
        Task<IEnumerable<ConfigEntry>> GetProjectConfigAsync(Guid projectId);
        Task UpdateProjectConfigAsync(Guid projectId, Dictionary<string, string> newConfig);
        Task CreateProjectAsync(ConfigProject project);
      //  Task UpdateConfigAsync(string projectName, string key, string value);
      Task UpdateConfigAsync(Guid projectId, Dictionary<string, string> newConfig);
    }
}
