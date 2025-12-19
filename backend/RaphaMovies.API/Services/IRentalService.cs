using RaphaMovies.API.DTOs;

namespace RaphaMovies.API.Services;

public interface IRentalService
{
    Task<List<RentalDto>> GetUserRentalsAsync(Guid userId);
    Task<RentalDto?> GetRentalByIdAsync(Guid id);
    Task<RentalDto> CreateRentalAsync(Guid userId, CreateRentalDto dto);
    Task<RentalDto?> ReturnRentalAsync(Guid id, Guid userId);
    Task<PaginatedResponseDto<RentalDto>> GetAllRentalsAsync(int page = 1, int pageSize = 20);
}
