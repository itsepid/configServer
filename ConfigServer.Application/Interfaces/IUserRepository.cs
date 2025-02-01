using System.Threading.Tasks;
using ConfigServer.Domain.Entities;
namespace ConfigServer.Application.Interfaces
{
public interface IUserRepository
{
    Task<User> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
}