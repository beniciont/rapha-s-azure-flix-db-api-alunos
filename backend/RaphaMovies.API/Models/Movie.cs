using System.ComponentModel.DataAnnotations;

namespace RaphaMovies.API.Models;

public class Movie
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Synopsis { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(500)]
    public string BackdropUrl { get; set; } = string.Empty;

    [StringLength(500)]
    public string? TrailerUrl { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    [StringLength(20)]
    public string Duration { get; set; } = string.Empty;

    [Range(0, 10)]
    public decimal Rating { get; set; }

    [Required]
    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Range(0, 1000)]
    public decimal RentalPrice { get; set; } = 9.99m;

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
