using System.ComponentModel.DataAnnotations;

namespace NoobLeagueAPI.DTOs.UserDTOs;

public class UserCreateRequestDTO : UserBaseDTO
{
    // Herdou o Nickname. Podemos adicionar validações específicas de entrada aqui se quisermos
    [Required(ErrorMessage = "O nickname é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nickname não pode passar de 50 caracteres.")]
    public new string Nickname { get; set; } = string.Empty;
}