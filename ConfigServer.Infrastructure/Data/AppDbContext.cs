using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
namespace ConfigServer.Infrastructure.Data {
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } 
}
}
