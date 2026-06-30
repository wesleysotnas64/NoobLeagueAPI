using Microsoft.AspNetCore.Mvc;
using NoobLeagueAPI.DTOs.UserTournamentDTOs;
using NoobLeagueAPI.Services;

namespace NoobLeagueAPI.Controllers;

[ApiController]
[Route("user-tournaments-api/")]
public class UserTournamentsController : ControllerBase
{
    private readonly UserTournamentService _userTournamentService;

    public UserTournamentsController(UserTournamentService userTournamentService)
    {
        _userTournamentService = userTournamentService;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribePlayer([FromBody] UserTournamentCreateRequestDTO request)
    {
        try
        {
            await _userTournamentService.SubscribePlayerAsync(request);

            // Como é uma tabela de junção puramente de IDs, retornar uma mensagem de sucesso 
            // ou o próprio request com um status 200/201 é o ideal para o front-end.
            return Ok(new { message = "Jogador inscrito com sucesso no torneio!" });
        }
        catch (Exception ex) when (ex.Message.Contains("não encontrado"))
        {
            // Captura os erros de usuário ou torneio inexistente e retorna 404
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Captura duplicidade ou status inválido do torneio e retorna 400
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("tournament/{tournamentId:guid}")]
    public async Task<IActionResult> GetUsersByTournament(Guid tournamentId)
    {
        try
        {
            var result = await _userTournamentService.GetUsersByTournamentIdAsync(tournamentId);

            if (result == null)
            {
                return NotFound(new { message = "Torneio não encontrado ou desativado." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            // Tratamento genérico para capturar qualquer falha inesperada no banco
            return StatusCode(500, new { message = "Erro interno ao buscar os dados do torneio.", error = ex.Message });
        }
    }
}