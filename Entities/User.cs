namespace NoobLeagueAPI.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nickname { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public DateTime? DeletionDate { get; set; }
}
