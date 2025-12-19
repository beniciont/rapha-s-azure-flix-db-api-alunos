using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaphaMovies.API.Models;

public class Rental
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid MovieId { get; set; }

    public DateTime RentedAt { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedAt { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "active"; // "active", "returned", "overdue"

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; } = null!;
}
