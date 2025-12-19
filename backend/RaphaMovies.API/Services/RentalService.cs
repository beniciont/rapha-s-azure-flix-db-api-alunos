using Microsoft.EntityFrameworkCore;
using RaphaMovies.API.Data;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Models;

namespace RaphaMovies.API.Services;

public class RentalService : IRentalService
{
    private readonly ApplicationDbContext _context;

    public RentalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RentalDto>> GetUserRentalsAsync(Guid userId)
    {
        var rentals = await _context.Rentals
            .Include(r => r.Movie)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RentedAt)
            .ToListAsync();

        // Update overdue status
        foreach (var rental in rentals.Where(r => r.Status == "active" && r.DueDate < DateTime.UtcNow))
        {
            rental.Status = "overdue";
        }
        await _context.SaveChangesAsync();

        return rentals.Select(MapToDto).ToList();
    }

    public async Task<RentalDto?> GetRentalByIdAsync(Guid id)
    {
        var rental = await _context.Rentals
            .Include(r => r.Movie)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null) return null;

        // Update overdue status
        if (rental.Status == "active" && rental.DueDate < DateTime.UtcNow)
        {
            rental.Status = "overdue";
            await _context.SaveChangesAsync();
        }

        return MapToDto(rental);
    }

    public async Task<RentalDto> CreateRentalAsync(Guid userId, CreateRentalDto dto)
    {
        // Check if movie exists
        var movie = await _context.Movies.FindAsync(dto.MovieId);
        if (movie == null)
        {
            throw new InvalidOperationException("Filme não encontrado");
        }

        if (!movie.IsAvailable)
        {
            throw new InvalidOperationException("Filme não está disponível para aluguel");
        }

        // Check if user already has an active rental for this movie
        var existingRental = await _context.Rentals
            .FirstOrDefaultAsync(r => 
                r.UserId == userId && 
                r.MovieId == dto.MovieId && 
                (r.Status == "active" || r.Status == "overdue"));

        if (existingRental != null)
        {
            throw new InvalidOperationException("Você já possui um aluguel ativo deste filme");
        }

        var rental = new Rental
        {
            UserId = userId,
            MovieId = dto.MovieId,
            RentedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(dto.RentalDays),
            Status = "active",
            TotalPrice = movie.RentalPrice * dto.RentalDays / 7 // Prorate based on days
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        // Load movie for response
        await _context.Entry(rental).Reference(r => r.Movie).LoadAsync();

        return MapToDto(rental);
    }

    public async Task<RentalDto?> ReturnRentalAsync(Guid id, Guid userId)
    {
        var rental = await _context.Rentals
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null) return null;

        // Verify ownership (unless admin - handled in controller)
        if (rental.UserId != userId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para devolver este aluguel");
        }

        if (rental.Status == "returned")
        {
            throw new InvalidOperationException("Este aluguel já foi devolvido");
        }

        rental.ReturnedAt = DateTime.UtcNow;
        rental.Status = "returned";

        await _context.SaveChangesAsync();

        return MapToDto(rental);
    }

    public async Task<PaginatedResponseDto<RentalDto>> GetAllRentalsAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Rentals
            .Include(r => r.Movie)
            .Include(r => r.User)
            .OrderByDescending(r => r.RentedAt);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var rentals = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Update overdue status
        foreach (var rental in rentals.Where(r => r.Status == "active" && r.DueDate < DateTime.UtcNow))
        {
            rental.Status = "overdue";
        }
        await _context.SaveChangesAsync();

        return new PaginatedResponseDto<RentalDto>
        {
            Data = rentals.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1
        };
    }

    private static RentalDto MapToDto(Rental rental)
    {
        return new RentalDto
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MovieId = rental.MovieId,
            RentedAt = rental.RentedAt,
            DueDate = rental.DueDate,
            ReturnedAt = rental.ReturnedAt,
            Status = rental.Status,
            TotalPrice = rental.TotalPrice,
            Movie = rental.Movie != null ? new MovieDto
            {
                Id = rental.Movie.Id,
                Title = rental.Movie.Title,
                Synopsis = rental.Movie.Synopsis,
                ImageUrl = rental.Movie.ImageUrl,
                BackdropUrl = rental.Movie.BackdropUrl,
                TrailerUrl = rental.Movie.TrailerUrl,
                Year = rental.Movie.Year,
                Duration = rental.Movie.Duration,
                Rating = rental.Movie.Rating,
                Genre = rental.Movie.Genre,
                RentalPrice = rental.Movie.RentalPrice,
                IsAvailable = rental.Movie.IsAvailable,
                CreatedAt = rental.Movie.CreatedAt,
                UpdatedAt = rental.Movie.UpdatedAt
            } : null,
            User = rental.User != null ? new UserDto
            {
                Id = rental.User.Id,
                Email = rental.User.Email,
                Name = rental.User.Name,
                CreatedAt = rental.User.CreatedAt,
                UpdatedAt = rental.User.UpdatedAt
            } : null
        };
    }
}
