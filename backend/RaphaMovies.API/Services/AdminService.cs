using Microsoft.EntityFrameworkCore;
using RaphaMovies.API.Data;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Models;

namespace RaphaMovies.API.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;

    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminStatsDto> GetStatsAsync()
    {
        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalUsers = await _context.Users.CountAsync();
        var totalMovies = await _context.Movies.CountAsync();
        
        var activeRentals = await _context.Rentals
            .CountAsync(r => r.Status == "active");
        
        var overdueRentals = await _context.Rentals
            .CountAsync(r => r.Status == "overdue" || 
                (r.Status == "active" && r.DueDate < now));

        var totalRevenue = await _context.Rentals
            .Where(r => r.Status == "returned")
            .SumAsync(r => r.TotalPrice);

        var monthlyRevenue = await _context.Rentals
            .Where(r => r.Status == "returned" && r.ReturnedAt >= firstDayOfMonth)
            .SumAsync(r => r.TotalPrice);

        return new AdminStatsDto
        {
            TotalUsers = totalUsers,
            TotalMovies = totalMovies,
            ActiveRentals = activeRentals,
            OverdueRentals = overdueRentals,
            TotalRevenue = totalRevenue,
            MonthlyRevenue = monthlyRevenue
        };
    }

    public async Task<PaginatedResponseDto<UserDto>> GetUsersAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Users.OrderBy(u => u.Name);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponseDto<UserDto>
        {
            Data = users.Select(MapToDto).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;
        return MapToDto(user);
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return null;

        if (dto.Name != null) user.Name = dto.Name;
        if (dto.Email != null) user.Email = dto.Email.ToLower();

        if (dto.Roles != null)
        {
            // Remove existing roles and add new ones
            _context.UserRoles.RemoveRange(user.Roles);
            foreach (var role in dto.Roles)
            {
                user.Roles.Add(new UserRole { UserId = user.Id, Role = role });
            }
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
