using Microsoft.AspNetCore.Mvc;
using NoobLeagueAPI.DTOs.TournamentDTOs;
using NoobLeagueAPI.Enums;
using NoobLeagueAPI.Services;

namespace NoobLeagueAPI.Controllers;

[ApiController]
[Route("tournament-api")]
public class TournamentController : ControllerBase
{
    private readonly TournamentService _tournamentService;

    public TournamentController(TournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var tournaments = await _tournamentService.GetAllActiveAsync();
        return Ok(tournaments);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null)
            return NotFound(new { message = "Torneio não encontrado." });

        return Ok(tournament);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TournamentCreateRequestDTO request)
    {
        if (request == null)
            return BadRequest(new { message = "Dados inválidos." });

        var newTournament = await _tournamentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = newTournament.Id }, newTournament);
    }

    // Endpoint para os ADMs mudarem manualmente o estado do campeonato
    [HttpPatch("update-status/{id:guid}")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] TournamentStatus status)
    {
        var result = await _tournamentService.UpdateStatusAsync(id, status);
        if (!result)
            return NotFound(new { message = "Torneio não encontrado para atualizar o status." });

        return Ok(new { message = $"Status do torneio atualizado para {status} com sucesso." });
    }

    [HttpDelete("delete/soft/{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var result = await _tournamentService.SoftDeleteAsync(id);
        if (!result)
            return NotFound(new { message = "Torneio não encontrado para exclusão." });

        return Ok(new { message = "Torneio desativado com sucesso." });
    }
}