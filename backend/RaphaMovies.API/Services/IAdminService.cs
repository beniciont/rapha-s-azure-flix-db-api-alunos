using RaphaMovies.API.DTOs;

namespace RaphaMovies.API.Services;

public interface IAdminService
{
    Task<AdminStatsDto> GetStatsAsync();
    Task<PaginatedResponseDto<UserDto>> GetUsersAsync(int page = 1, int pageSize = 20);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteUserAsync(Guid id);
}
