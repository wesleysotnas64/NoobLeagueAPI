using NoobLeagueAPI.Entities;

namespace NoobLeagueAPI.Entities;

public class UserTournament
{
    public Guid UserId { get; set; }
    public Guid TournamentId { get; set; }
    public bool IsChampion { get; set; } = false;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}