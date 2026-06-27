using Microsoft.AspNetCore.Mvc;
using NoobLeagueAPI.DTOs.UserDTOs;
using NoobLeagueAPI.Services;

namespace NoobLeagueAPI.Controllers;

[ApiController]
[Route("user-api")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllActiveUsersAsync();
        return Ok(users);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { message = "Usuário não encontrado ou inativo." });

        return Ok(user);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] UserCreateRequestDTO request)
    {
        // O .NET valida o DTO automaticamente, mas uma checagem defensiva é sempre boa
        if (request == null)
            return BadRequest(new { message = "Os dados do usuário não podem ser nulos." });

        var newUserResponse = await _userService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = newUserResponse.Id }, newUserResponse);
    }

    [HttpDelete("delete/soft/{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var result = await _userService.SoftDeleteAsync(id);

        if (!result)
            return NotFound(new { message = "Usuário não encontrado para exclusão lógica." });

        return Ok(new { message = "Usuário desativado com sucesso (Soft Delete)." });
    }

    [HttpDelete("delete/hard/{id:guid}")]
    public async Task<IActionResult> HardDelete(Guid id)
    {
        var result = await _userService.HardDeleteAsync(id);

        if (!result)
            return NotFound(new { message = "Usuário não encontrado para exclusão definitiva." });

        return Ok(new { message = "Usuário removido permanentemente do banco de dados (Hard Delete)." });
    }
}