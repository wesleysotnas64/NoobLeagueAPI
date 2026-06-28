using System.ComponentModel.DataAnnotations;

namespace NoobLeagueAPI.DTOs.TournamentDTOs;

public class TournamentCreateRequestDTO : TournamentBaseDTO
{
    [Required(ErrorMessage = "O nome do torneio é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode passar de 100 caracteres.")]
    public new string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data do evento é obrigatória.")]
    public new DateTime EventDate { get; set; }
}
