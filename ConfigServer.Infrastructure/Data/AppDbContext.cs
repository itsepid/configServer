using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
namespace ConfigServer.Infrastructure.Data
{
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } 

    public DbSet<Config> Configs { get; set;}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Config>()
        .HasOne<User>() 
        .WithMany() 
        .HasForeignKey(c => c.UserId) 
        .OnDelete(DeleteBehavior.Cascade);
    }


}
}
