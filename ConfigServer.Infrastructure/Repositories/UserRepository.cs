using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
using ConfigServer.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using ConfigServer.Infrastructure.Data;


namespace ConfigServer.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByUsernameAsync(string username)
{
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);  
}

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
