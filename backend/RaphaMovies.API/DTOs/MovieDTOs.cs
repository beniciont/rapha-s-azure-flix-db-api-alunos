using System.ComponentModel.DataAnnotations;

namespace RaphaMovies.API.DTOs;

public class MovieDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string BackdropUrl { get; set; } = string.Empty;
    public string? TrailerUrl { get; set; }
    public int Year { get; set; }
    public string Duration { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string Genre { get; set; } = string.Empty;
    public decimal RentalPrice { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateMovieDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sinopse é obrigatória")]
    [StringLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
    public string Synopsis { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL da imagem é obrigatória")]
    [Url(ErrorMessage = "URL da imagem inválida")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL do backdrop é obrigatória")]
    [Url(ErrorMessage = "URL do backdrop inválida")]
    public string BackdropUrl { get; set; } = string.Empty;

    [Url(ErrorMessage = "URL do trailer inválida")]
    public string? TrailerUrl { get; set; }

    [Required(ErrorMessage = "Ano é obrigatório")]
    [Range(1900, 2100, ErrorMessage = "Ano inválido")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Duração é obrigatória")]
    [StringLength(20, ErrorMessage = "Duração deve ter no máximo 20 caracteres")]
    public string Duration { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gênero é obrigatório")]
    [StringLength(50, ErrorMessage = "Gênero deve ter no máximo 50 caracteres")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Preço do aluguel é obrigatório")]
    [Range(0.01, 1000, ErrorMessage = "Preço deve estar entre 0.01 e 1000")]
    public decimal RentalPrice { get; set; }
}

public class UpdateMovieDto
{
    [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string? Title { get; set; }

    [StringLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
    public string? Synopsis { get; set; }

    [Url(ErrorMessage = "URL da imagem inválida")]
    public string? ImageUrl { get; set; }

    [Url(ErrorMessage = "URL do backdrop inválida")]
    public string? BackdropUrl { get; set; }

    [Url(ErrorMessage = "URL do trailer inválida")]
    public string? TrailerUrl { get; set; }

    [Range(1900, 2100, ErrorMessage = "Ano inválido")]
    public int? Year { get; set; }

    [StringLength(20, ErrorMessage = "Duração deve ter no máximo 20 caracteres")]
    public string? Duration { get; set; }

    [StringLength(50, ErrorMessage = "Gênero deve ter no máximo 50 caracteres")]
    public string? Genre { get; set; }

    [Range(0.01, 1000, ErrorMessage = "Preço deve estar entre 0.01 e 1000")]
    public decimal? RentalPrice { get; set; }

    public bool? IsAvailable { get; set; }
}

public class SearchMoviesQueryDto
{
    public string? Query { get; set; }
    public string? Genre { get; set; }
    public int? Year { get; set; }
    public decimal? MinRating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "title";
    public string SortOrder { get; set; } = "asc";
}
