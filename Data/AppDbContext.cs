using Microsoft.EntityFrameworkCore;
using NoobLeagueAPI.Entities;

namespace NoobLeagueAPI.Data;

public class AppDbContext : DbContext
{
    // Certifique-se de que o construtor recebe exatamente "DbContextOptions<AppDbContext>"
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
        });
    }
}
