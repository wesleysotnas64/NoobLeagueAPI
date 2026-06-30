using NoobLeagueAPI.DTOs.TournamentDTOs;
using NoobLeagueAPI.DTOs.UserDTOs;

namespace NoobLeagueAPI.DTOs.UserTournamentDTOs;

public class TournamentUserDetailDTO
{
    public TournamentResponseDTO Tournament { get; set; } = null!;
    public List<UserResponseDTO> Users { get; set; } = new();
}