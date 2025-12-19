using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaphaMovies.API.Models;

public class UserRole
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "user"; // "admin" or "user"

    // Navigation property
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
