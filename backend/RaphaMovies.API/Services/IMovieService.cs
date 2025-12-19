using RaphaMovies.API.DTOs;

namespace RaphaMovies.API.Services;

public interface IMovieService
{
    Task<PaginatedResponseDto<MovieDto>> GetAllMoviesAsync(int page = 1, int pageSize = 20);
    Task<MovieDto?> GetMovieByIdAsync(Guid id);
    Task<PaginatedResponseDto<MovieDto>> GetMoviesByGenreAsync(string genre, int page = 1, int pageSize = 20);
    Task<PaginatedResponseDto<MovieDto>> SearchMoviesAsync(SearchMoviesQueryDto query);
    Task<List<string>> GetGenresAsync();
    Task<MovieDto> CreateMovieAsync(CreateMovieDto dto);
    Task<MovieDto?> UpdateMovieAsync(Guid id, UpdateMovieDto dto);
    Task<bool> DeleteMovieAsync(Guid id);
}
