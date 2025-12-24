using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Services;

namespace RaphaMovies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IMovieService _movieService;
    private readonly IRentalService _rentalService;

    public AdminController(
        IAdminService adminService, 
        IMovieService movieService,
        IRentalService rentalService)
    {
        _adminService = adminService;
        _movieService = movieService;
        _rentalService = rentalService;
    }

    /// <summary>
    /// Retorna estatísticas do sistema
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(AdminStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _adminService.GetStatsAsync();
        return Ok(stats);
    }

    /// <summary>
    /// Lista todos os usuários
    /// </summary>
    [HttpGet("users")]
    [ProducesResponseType(typeof(PaginatedResponseDto<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _adminService.GetUsersAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Busca um usuário pelo ID
    /// </summary>
    [HttpGet("users/{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _adminService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado", code = "USER_NOT_FOUND" });
        }
        return Ok(user);
    }

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    [HttpPut("users/{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        var user = await _adminService.UpdateUserAsync(id, dto);
        if (user == null)
        {
            return NotFound(new { message = "Usuário não encontrado", code = "USER_NOT_FOUND" });
        }
        return Ok(user);
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    [HttpDelete("users/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _adminService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Usuário não encontrado", code = "USER_NOT_FOUND" });
        }
        return NoContent();
    }

    /// <summary>
    /// Lista todos os filmes (admin view)
    /// </summary>
    [HttpGet("movies")]
    [ProducesResponseType(typeof(PaginatedResponseDto<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMovies([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _movieService.GetAllMoviesAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Lista todos os aluguéis (admin view)
    /// </summary>
    [HttpGet("rentals")]
    [ProducesResponseType(typeof(PaginatedResponseDto<RentalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRentals([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _rentalService.GetAllRentalsAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Popula o banco de dados com filmes de exemplo
    /// </summary>
    /// <param name="force">Se true, adiciona filmes mesmo se já existirem dados</param>
    [HttpPost("seed")]
    [ProducesResponseType(typeof(SeedResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SeedDatabase([FromQuery] bool force = false)
    {
        var result = await _adminService.SeedDatabaseAsync(force);
        return Ok(result);
    }
}
