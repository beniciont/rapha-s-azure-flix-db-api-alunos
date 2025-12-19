using RaphaMovies.API.DTOs;

namespace RaphaMovies.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    Task<UserDto?> GetCurrentUserAsync(Guid userId);
}
