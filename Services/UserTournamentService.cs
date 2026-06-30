using Microsoft.EntityFrameworkCore;
using NoobLeagueAPI.Data;
using NoobLeagueAPI.DTOs.TournamentDTOs;
using NoobLeagueAPI.DTOs.UserDTOs;
using NoobLeagueAPI.DTOs.UserTournamentDTOs;
using NoobLeagueAPI.Entities;
using NoobLeagueAPI.Enums;

namespace NoobLeagueAPI.Services;

public class UserTournamentService
{
    private readonly AppDbContext _context;

    public UserTournamentService(AppDbContext context)
    {
        _context = context;
    }

    // 1. EFETUAR INSCRIÇÃO (POST)
    public async Task<bool> SubscribePlayerAsync(UserTournamentCreateRequestDTO request)
    {
        // Validação 1: Verificar se o usuário existe e não está deletado
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId && u.DeletionDate == null);

        if (!userExists)
            throw new Exception("Usuário não encontrado.");

        // Validação 2: Verificar se o torneio existe e não está deletado
        var tournament = await _context.Tournaments
            .FirstOrDefaultAsync(t => t.Id == request.TournamentId && t.DeletionDate == null);

        if (tournament == null)
            throw new Exception("Torneio não encontrado.");

        // Validação 3: Verificar se o torneio aceita inscrições
        if (tournament.Status != TournamentStatus.InscricoesAbertas) // Ou o enum correto de aberto
            throw new Exception("Este torneio não está aceitando inscrições.");

        // Validação 4: Verificar duplicidade
        var alreadySubscribed = await _context.UserTournaments
            .AnyAsync(ut => ut.UserId == request.UserId && ut.TournamentId == request.TournamentId);

        if (alreadySubscribed)
            throw new Exception("O jogador já está inscrito neste torneio.");

        // Salva a nova relação
        var userTournament = new UserTournament
        {
            UserId = request.UserId,
            TournamentId = request.TournamentId
        };

        _context.UserTournaments.Add(userTournament);
        await _context.SaveChangesAsync();

        return true;
    }

    // 2. OBTER JOGADORES DE UM TORNEIO (GET BY TOURNAMENT ID)
    public async Task<TournamentUserDetailDTO?> GetUsersByTournamentIdAsync(Guid tournamentId)
    {
        // 1. Buscamos o torneio ativo primeiro
        var tournament = await _context.Tournaments
            .Where(t => t.Id == tournamentId && t.DeletionDate == null)
            .Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Name = t.Name,
                EventDate = t.EventDate,
                Status = t.Status
            })
            .FirstOrDefaultAsync();

        // Se o torneio não existir (ou foi deletado), retornamos null imediatamente
        if (tournament == null)
            return null;

        // 2. Buscamos todos os usuários inscritos que estão ativos
        // Fazemos um JOIN manual via LINQ entre as tabelas UserTournaments e Users
        var usersInscribed = await _context.UserTournaments
            .Where(ut => ut.TournamentId == tournamentId)
            .Join(
                _context.Users.Where(u => u.DeletionDate == null), // Tabela destino (apenas usuários ativos)
                userTournament => userTournament.UserId,           // Chave na tabela de junção
                user => user.Id,                                   // Chave na tabela de Usuários
                (userTournament, user) => new UserResponseDTO      // Como projetar o resultado
                {
                    Id = user.Id,
                    Nickname = user.Nickname,
                    CreationDate = user.CreationDate
                }
            )
            .ToListAsync();

        // 3. Juntamos tudo no DTO que você desenhou e retornamos
        return new TournamentUserDetailDTO
        {
            Tournament = tournament,
            Users = usersInscribed
        };
    }

    // 3. OBTER TORNEIOS DE UM JOGADOR (GET BY USER ID)
    public async Task<UserTournamentDetailDTO?> GetTournamentsByUserIdAsync(Guid userId)
    {
        // Aqui buscaremos o Usuário e faremos um Join/Include com a tabela de torneios
        // para preencher a lista de Tournaments do seu DTO.
        return null;
    }
}