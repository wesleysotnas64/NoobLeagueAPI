namespace NoobLeagueAPI.DTOs.UserDTOs;

public class UserBaseDTO
{
    // Toda operação de usuário (criar, atualizar, listar) vai precisar do Nickname
    public string Nickname { get; set; } = string.Empty;
}