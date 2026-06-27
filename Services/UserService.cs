using Microsoft.EntityFrameworkCore;
using NoobLeagueAPI.Data;
using NoobLeagueAPI.DTOs.UserDTOs;
using NoobLeagueAPI.Entities;

namespace NoobLeagueAPI.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    // Agora retorna uma lista de DTOs de resposta
    public async Task<List<UserResponseDTO>> GetAllActiveUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.DeletionDate == null)
            .OrderByDescending(u => u.CreationDate)
            .Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Nickname = u.Nickname,
                CreationDate = u.CreationDate
            })
            .ToListAsync();
    }

    // Agora retorna um DTO de resposta específico
    public async Task<UserResponseDTO?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id && u.DeletionDate == null)
            .Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Nickname = u.Nickname,
                CreationDate = u.CreationDate
            })
            .FirstOrDefaultAsync();
    }

    // Aceita o Request DTO e retorna o Response DTO criado
    public async Task<UserResponseDTO> CreateAsync(UserCreateRequestDTO request)
    {
        var user = new User
        {
            Nickname = request.Nickname
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponseDTO
        {
            Id = user.Id,
            Nickname = user.Nickname,
            CreationDate = user.CreationDate
        };
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.DeletionDate == null);
        if (user == null) return false;

        user.DeletionDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}