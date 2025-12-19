using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Services;

namespace RaphaMovies.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    /// <summary>
    /// Lista todos os filmes com paginação
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _movieService.GetAllMoviesAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Busca um filme pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null)
        {
            return NotFound(new { message = "Filme não encontrado", code = "MOVIE_NOT_FOUND" });
        }
        return Ok(movie);
    }

    /// <summary>
    /// Lista filmes por gênero
    /// </summary>
    [HttpGet("genre/{genre}")]
    [ProducesResponseType(typeof(PaginatedResponseDto<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByGenre(string genre, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _movieService.GetMoviesByGenreAsync(genre, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Pesquisa filmes com filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedResponseDto<MovieDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchMoviesQueryDto query)
    {
        var result = await _movieService.SearchMoviesAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// Lista todos os gêneros disponíveis
    /// </summary>
    [HttpGet("genres")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _movieService.GetGenresAsync();
        return Ok(genres);
    }

    /// <summary>
    /// Cria um novo filme (admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateMovieDto dto)
    {
        var movie = await _movieService.CreateMovieAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    /// <summary>
    /// Atualiza um filme (admin only)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieDto dto)
    {
        var movie = await _movieService.UpdateMovieAsync(id, dto);
        if (movie == null)
        {
            return NotFound(new { message = "Filme não encontrado", code = "MOVIE_NOT_FOUND" });
        }
        return Ok(movie);
    }

    /// <summary>
    /// Remove um filme (admin only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _movieService.DeleteMovieAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Filme não encontrado", code = "MOVIE_NOT_FOUND" });
        }
        return NoContent();
    }
}
