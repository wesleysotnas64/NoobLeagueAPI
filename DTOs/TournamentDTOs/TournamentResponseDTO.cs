using NoobLeagueAPI.Enums;

namespace NoobLeagueAPI.DTOs.TournamentDTOs;

public class TournamentResponseDTO : TournamentBaseDTO
{
    public Guid Id { get; set; }
    public TournamentStatus Status { get; set; }
    public string StatusDescription => Status.ToString(); // Facilita a leitura no front-end
    public DateTime CreationDate { get; set; }
}
