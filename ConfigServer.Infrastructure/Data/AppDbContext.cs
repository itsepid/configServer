using Microsoft.EntityFrameworkCore;
using ConfigServer.Domain.Entities;
namespace ConfigServer.Infrastructure.Data
{
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } 

    public DbSet<Config> Configs { get; set;}

    public DbSet<ConfigProject> ConfigProjects { get; set; } 
        public DbSet<ConfigEntry> ConfigEntries { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Config>()
        .HasOne<User>() 
        .WithMany() 
        .HasForeignKey(c => c.UserId) 
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ConfigProject>()
                .HasIndex(p => p.ProjectName)
                .IsUnique(); 

            
            modelBuilder.Entity<ConfigEntry>()
                .HasIndex(c => new { c.ConfigProjectId, c.Key })
                .IsUnique(); 
    
    }


}
}
