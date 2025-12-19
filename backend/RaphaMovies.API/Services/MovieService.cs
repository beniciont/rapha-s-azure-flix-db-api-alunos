using Microsoft.EntityFrameworkCore;
using RaphaMovies.API.Data;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Models;

namespace RaphaMovies.API.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;

    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponseDto<MovieDto>> GetAllMoviesAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Movies.AsQueryable();
        return await GetPaginatedMoviesAsync(query, page, pageSize);
    }

    public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return null;
        return MapToDto(movie);
    }

    public async Task<PaginatedResponseDto<MovieDto>> GetMoviesByGenreAsync(string genre, int page = 1, int pageSize = 20)
    {
        var query = _context.Movies.Where(m => m.Genre.ToLower() == genre.ToLower());
        return await GetPaginatedMoviesAsync(query, page, pageSize);
    }

    public async Task<PaginatedResponseDto<MovieDto>> SearchMoviesAsync(SearchMoviesQueryDto searchQuery)
    {
        var query = _context.Movies.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchQuery.Query))
        {
            var searchTerm = searchQuery.Query.ToLower();
            query = query.Where(m => 
                m.Title.ToLower().Contains(searchTerm) || 
                m.Synopsis.ToLower().Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(searchQuery.Genre))
        {
            query = query.Where(m => m.Genre.ToLower() == searchQuery.Genre.ToLower());
        }

        if (searchQuery.Year.HasValue)
        {
            query = query.Where(m => m.Year == searchQuery.Year.Value);
        }

        if (searchQuery.MinRating.HasValue)
        {
            query = query.Where(m => m.Rating >= searchQuery.MinRating.Value);
        }

        // Apply sorting
        query = searchQuery.SortBy?.ToLower() switch
        {
            "year" => searchQuery.SortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(m => m.Year) 
                : query.OrderBy(m => m.Year),
            "rating" => searchQuery.SortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(m => m.Rating) 
                : query.OrderBy(m => m.Rating),
            "createdat" => searchQuery.SortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(m => m.CreatedAt) 
                : query.OrderBy(m => m.CreatedAt),
            _ => searchQuery.SortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(m => m.Title) 
                : query.OrderBy(m => m.Title)
        };

        return await GetPaginatedMoviesAsync(query, searchQuery.Page, searchQuery.PageSize);
    }

    public async Task<List<string>> GetGenresAsync()
    {
        return await _context.Movies
            .Select(m => m.Genre)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync();
    }

    public async Task<MovieDto> CreateMovieAsync(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Synopsis = dto.Synopsis,
            ImageUrl = dto.ImageUrl,
            BackdropUrl = dto.BackdropUrl,
            TrailerUrl = dto.TrailerUrl,
            Year = dto.Year,
            Duration = dto.Duration,
            Genre = dto.Genre,
            RentalPrice = dto.RentalPrice,
            Rating = 0,
            IsAvailable = true
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return MapToDto(movie);
    }

    public async Task<MovieDto?> UpdateMovieAsync(Guid id, UpdateMovieDto dto)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return null;

        // Update only provided fields
        if (dto.Title != null) movie.Title = dto.Title;
        if (dto.Synopsis != null) movie.Synopsis = dto.Synopsis;
        if (dto.ImageUrl != null) movie.ImageUrl = dto.ImageUrl;
        if (dto.BackdropUrl != null) movie.BackdropUrl = dto.BackdropUrl;
        if (dto.TrailerUrl != null) movie.TrailerUrl = dto.TrailerUrl;
        if (dto.Year.HasValue) movie.Year = dto.Year.Value;
        if (dto.Duration != null) movie.Duration = dto.Duration;
        if (dto.Genre != null) movie.Genre = dto.Genre;
        if (dto.RentalPrice.HasValue) movie.RentalPrice = dto.RentalPrice.Value;
        if (dto.IsAvailable.HasValue) movie.IsAvailable = dto.IsAvailable.Value;

        movie.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDto(movie);
    }

    public async Task<bool> DeleteMovieAsync(Guid id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<PaginatedResponseDto<MovieDto>> GetPaginatedMoviesAsync(
        IQueryable<Movie> query, int page, int pageSize)
    {
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var movies = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<MovieDto>
        {
            Data = movies.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1
        };
    }

    private static MovieDto MapToDto(Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Synopsis = movie.Synopsis,
            ImageUrl = movie.ImageUrl,
            BackdropUrl = movie.BackdropUrl,
            TrailerUrl = movie.TrailerUrl,
            Year = movie.Year,
            Duration = movie.Duration,
            Rating = movie.Rating,
            Genre = movie.Genre,
            RentalPrice = movie.RentalPrice,
            IsAvailable = movie.IsAvailable,
            CreatedAt = movie.CreatedAt,
            UpdatedAt = movie.UpdatedAt
        };
    }
}
