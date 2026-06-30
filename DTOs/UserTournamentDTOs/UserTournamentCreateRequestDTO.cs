namespace NoobLeagueAPI.DTOs.UserTournamentDTOs;

public class UserTournamentCreateRequestDTO
{
    public Guid TournamentId { get; set; }
    public Guid UserId { get; set; }
}