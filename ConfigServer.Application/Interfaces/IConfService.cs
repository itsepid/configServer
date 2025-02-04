using System.Collections.Generic;
using ConfigServer.Domain.Entities;
using System.Threading.Tasks;

namespace ConfigServer.Application.Interfaces
{
    public interface IConfService
    {
        Task<bool> ValidatePasskeyAsync(string projectName, string passkey);
        Task<Dictionary<string, object>> GetProjectConfigAsync(string projectName);
        Task CreateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig);
         Task CreateProjectAsync(string projectName, string passkey, string environment);

     Task UpdateProjectConfigAsync(string projectName, Dictionary<string, string> newConfig);
}
}