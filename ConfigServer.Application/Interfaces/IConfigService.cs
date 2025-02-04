using ConfigServer.Domain.Entities;
using ConfigServer.Application.DTOs;
namespace ConfigServer.Application.Interfaces
{
public interface IConfigService
{
    Task<Config> CreateConfigAsync(ConfigDTO configDto, int userId, string userRole);
    Task UpdateConfigAsync(Guid configId, UpdateConfigDTO updatedConfigm, string userRole);
   Task DeleteConfigAsync(Guid configId, string userRole);

}
}
