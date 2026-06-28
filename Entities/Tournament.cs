using NoobLeagueAPI.Enums;

namespace NoobLeagueAPI.Entities;

public class Tournament
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public TournamentStatus Status { get; set; } = TournamentStatus.EmBreve;

    // Auditoria base padrão do seu projeto
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeletionDate { get; set; }
}
