using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaphaMovies.API.Data;
using RaphaMovies.API.Services;

namespace RaphaMovies.API.Controllers;

[ApiController]
[Route("api/setup")]
public class SetupController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAdminService _adminService;
    private readonly IConfiguration _configuration;

    public SetupController(ApplicationDbContext context, IAdminService adminService, IConfiguration configuration)
    {
        _context = context;
        _adminService = adminService;
        _configuration = configuration;
    }

    /// <summary>
    /// Setup inicial do sistema (apenas 1x): cria admin e popula filmes de exemplo.
    /// Protegido por uma chave de setup para evitar uso indevido.
    /// </summary>
    [HttpPost("init")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SeedResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Init([FromQuery] string key)
    {
        var expectedKey = _configuration["Setup:Key"] ?? Environment.GetEnvironmentVariable("SETUP_KEY");

        if (string.IsNullOrWhiteSpace(expectedKey))
        {
            return BadRequest(new
            {
                message = "Setup key não configurada. Defina Setup__Key ou SETUP_KEY no App Service para habilitar /api/setup/init.",
                code = "SETUP_KEY_NOT_CONFIGURED"
            });
        }

        if (string.IsNullOrWhiteSpace(key) || key != expectedKey)
        {
            return Unauthorized(new
            {
                message = "Setup key inválida.",
                code = "INVALID_SETUP_KEY"
            });
        }

        // Segurança: só permite setup se ainda não existir nenhum usuário
        var hasUsers = await _context.Users.AnyAsync();
        if (hasUsers)
        {
            return Conflict(new
            {
                message = "Setup já foi executado (já existem usuários no banco).",
                code = "SETUP_ALREADY_DONE"
            });
        }

        var result = await _adminService.SeedDatabaseAsync(force: false);
        return Ok(result);
    }
}
