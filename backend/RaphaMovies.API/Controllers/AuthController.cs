using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Services;
using System.Security.Claims;

namespace RaphaMovies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Faz login de um usuário
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message, code = "INVALID_CREDENTIALS" });
        }
    }

    /// <summary>
    /// Registra um novo usuário
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Me), response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message, code = "EMAIL_EXISTS" });
        }
    }

    /// <summary>
    /// Faz logout do usuário (client-side)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        // JWT is stateless, so logout is handled client-side
        return Ok(new { message = "Logout realizado com sucesso" });
    }

    /// <summary>
    /// Atualiza o token de acesso usando refresh token
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request.RefreshToken);
        if (response == null)
        {
            return Unauthorized(new { message = "Token inválido ou expirado", code = "INVALID_TOKEN" });
        }
        return Ok(response);
    }

    /// <summary>
    /// Retorna os dados do usuário autenticado
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _authService.GetCurrentUserAsync(Guid.Parse(userId));
        if (user == null)
        {
            return Unauthorized();
        }

        // Include roles in response
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new
        {
            user.Id,
            user.Email,
            user.Name,
            user.CreatedAt,
            user.UpdatedAt,
            Roles = roles
        });
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
