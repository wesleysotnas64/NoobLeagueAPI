using NoobLeagueAPI.Entities;

namespace NoobLeagueAPI.DTOs.UserTournamentDTOs;

public class UserTournamentDetailDTO
{
    public User User;
    public List<Tournament> Tournaments = [];
}
