using System.ComponentModel.DataAnnotations;

namespace RaphaMovies.API.DTOs;

public class RentalDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MovieId { get; set; }
    public DateTime RentedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public MovieDto? Movie { get; set; }
    public UserDto? User { get; set; }
}

public class CreateRentalDto
{
    [Required(ErrorMessage = "ID do filme é obrigatório")]
    public Guid MovieId { get; set; }

    [Range(1, 30, ErrorMessage = "Dias de aluguel deve estar entre 1 e 30")]
    public int RentalDays { get; set; } = 7;
}
