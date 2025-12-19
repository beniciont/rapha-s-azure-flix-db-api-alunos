using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Services;
using System.Security.Claims;

namespace RaphaMovies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalsController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    /// <summary>
    /// Lista os aluguéis do usuário autenticado
    /// </summary>
    [HttpGet("my-rentals")]
    [ProducesResponseType(typeof(List<RentalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyRentals()
    {
        var userId = GetUserId();
        var rentals = await _rentalService.GetUserRentalsAsync(userId);
        return Ok(rentals);
    }

    /// <summary>
    /// Busca um aluguel pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var rental = await _rentalService.GetRentalByIdAsync(id);
        if (rental == null)
        {
            return NotFound(new { message = "Aluguel não encontrado", code = "RENTAL_NOT_FOUND" });
        }

        // Check if user owns this rental or is admin
        var userId = GetUserId();
        var isAdmin = User.IsInRole("admin");

        if (rental.UserId != userId && !isAdmin)
        {
            return Forbid();
        }

        return Ok(rental);
    }

    /// <summary>
    /// Cria um novo aluguel
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRentalDto dto)
    {
        try
        {
            var userId = GetUserId();
            var rental = await _rentalService.CreateRentalAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message, code = "RENTAL_ERROR" });
        }
    }

    /// <summary>
    /// Devolve um aluguel
    /// </summary>
    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(typeof(RentalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Return(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var isAdmin = User.IsInRole("admin");

            // If admin, we need to get the rental first to check ownership
            if (isAdmin)
            {
                var existingRental = await _rentalService.GetRentalByIdAsync(id);
                if (existingRental == null)
                {
                    return NotFound(new { message = "Aluguel não encontrado", code = "RENTAL_NOT_FOUND" });
                }
                userId = existingRental.UserId;
            }

            var rental = await _rentalService.ReturnRentalAsync(id, userId);
            if (rental == null)
            {
                return NotFound(new { message = "Aluguel não encontrado", code = "RENTAL_NOT_FOUND" });
            }
            return Ok(rental);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message, code = "RENTAL_ERROR" });
        }
    }

    /// <summary>
    /// Lista todos os aluguéis (admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PaginatedResponseDto<RentalDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _rentalService.GetAllRentalsAsync(page, pageSize);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException();
        }
        return Guid.Parse(userIdClaim);
    }
}
