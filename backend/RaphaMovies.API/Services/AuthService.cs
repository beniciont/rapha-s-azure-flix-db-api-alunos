using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RaphaMovies.API.Data;
using RaphaMovies.API.DTOs;
using RaphaMovies.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RaphaMovies.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos");
        }

        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // Check if email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

        if (existingUser != null)
        {
            throw new InvalidOperationException("Este email já está cadastrado");
        }

        // Create new user
        var user = new User
        {
            Name = request.Name,
            Email = request.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        // Add default user role
        user.Roles.Add(new UserRole { UserId = user.Id, Role = "user" });

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateAuthResponse(user);
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        // In a real application, you would validate the refresh token against stored tokens
        // For simplicity, we're just decoding it here
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(GetJwtKey());

            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = GetJwtIssuer(),
                ValidAudience = GetJwtAudience(),
                ValidateLifetime = false // Don't validate lifetime for refresh token
            }, out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return null;

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            return GenerateAuthResponse(user);
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    private string GetJwtKey()
    {
        var key = _configuration["Jwt:Secret"] ?? _configuration["Jwt:Key"]; // Keep compatibility with both keys
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException("JWT Key/Secret not configured");

        return key;
    }

    private string GetJwtIssuer() => _configuration["Jwt:Issuer"] ?? "RaphaMovies";

    private string GetJwtAudience() => _configuration["Jwt:Audience"] ?? "RaphaMoviesApp";

    private AuthResponseDto GenerateAuthResponse(User user)
    {
        var expirationDays = int.Parse(_configuration["Jwt:ExpirationInDays"] ?? "7");
        var expiresAt = DateTime.UtcNow.AddDays(expirationDays);

        var token = GenerateJwtToken(user, expiresAt);
        var refreshToken = GenerateRefreshToken(user);

        return new AuthResponseDto
        {
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            },
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            Roles = user.Roles.Select(r => r.Role).ToList()
        };
    }

    private string GenerateJwtToken(User user, DateTime expiresAt)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtKey()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role));
        }

        var token = new JwtSecurityToken(
            issuer: GetJwtIssuer(),
            audience: GetJwtAudience(),
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(User user)
    {
        // Generate a refresh token with longer expiration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetJwtKey()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("token_type", "refresh")
        };

        var token = new JwtSecurityToken(
            issuer: GetJwtIssuer(),
            audience: GetJwtAudience(),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
