using Microsoft.EntityFrameworkCore;
using NoobLeagueAPI.Data;
using NoobLeagueAPI.DTOs.TournamentDTOs;
using NoobLeagueAPI.Entities;
using NoobLeagueAPI.Enums;

namespace NoobLeagueAPI.Services;

public class TournamentService
{
    private readonly AppDbContext _context;

    public TournamentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TournamentResponseDTO>> GetAllActiveAsync()
    {
        return await _context.Tournaments
            .AsNoTracking()
            .Where(t => t.DeletionDate == null)
            .OrderByDescending(t => t.EventDate)
            .Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Name = t.Name,
                EventDate = t.EventDate,
                Status = t.Status,
                CreationDate = t.CreationDate
            })
            .ToListAsync();
    }

    public async Task<TournamentResponseDTO?> GetByIdAsync(Guid id)
    {
        return await _context.Tournaments
            .AsNoTracking()
            .Where(t => t.Id == id && t.DeletionDate == null)
            .Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Name = t.Name,
                EventDate = t.EventDate,
                Status = t.Status,
                CreationDate = t.CreationDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TournamentResponseDTO> CreateAsync(TournamentCreateRequestDTO request)
    {
        var tournament = new Tournament
        {
            Name = request.Name,
            EventDate = request.EventDate.ToUniversalTime(), // Garante UTC para o Postgres
            Status = TournamentStatus.EmBreve
        };

        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        return new TournamentResponseDTO
        {
            Id = tournament.Id,
            Name = tournament.Name,
            EventDate = tournament.EventDate,
            Status = tournament.Status,
            CreationDate = tournament.CreationDate
        };
    }

    // Método para o ADM atualizar manualmente o status do campeonato
    public async Task<bool> UpdateStatusAsync(Guid id, TournamentStatus newStatus)
    {
        var tournament = await _context.Tournaments
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletionDate == null);

        if (tournament == null) return false;

        tournament.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var tournament = await _context.Tournaments
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletionDate == null);

        if (tournament == null) return false;

        tournament.DeletionDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
