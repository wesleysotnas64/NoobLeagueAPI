using Microsoft.EntityFrameworkCore;
using NoobLeagueAPI.Entities;

namespace NoobLeagueAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<UserTournament> UserTournaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento da entidade User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
        });

        // Mapeamento da nova entidade Tournament
        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            // Mapeia o Enum de status para ser salvo como número inteiro no banco
            entity.Property(e => e.Status).IsRequired().HasConversion<int>();
        });

        // Mapeamento da nova entidade de junção UserTournament
        modelBuilder.Entity<UserTournament>(entity =>
        {
            // Define a chave primária composta (UserId + TournamentId)
            entity.HasKey(e => new { e.UserId, e.TournamentId });

            entity.Property(e => e.IsChampion).IsRequired();
            entity.Property(e => e.RegistrationDate).IsRequired();
        });
    }
}