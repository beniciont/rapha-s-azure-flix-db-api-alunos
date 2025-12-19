namespace RaphaMovies.API.DTOs;

public class AdminStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalMovies { get; set; }
    public int ActiveRentals { get; set; }
    public int OverdueRentals { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
}

public class PaginatedResponseDto<T>
{
    public List<T> Data { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string>? Roles { get; set; }
}
