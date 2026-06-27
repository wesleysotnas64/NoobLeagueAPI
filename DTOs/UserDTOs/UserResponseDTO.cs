namespace NoobLeagueAPI.DTOs.UserDTOs;

public class UserResponseDTO : UserBaseDTO
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}