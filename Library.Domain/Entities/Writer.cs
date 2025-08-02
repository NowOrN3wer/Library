using Library.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public sealed class Writer : Entity
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [MaxLength(500)]
    public string? Biography { get; set; }

    [MaxLength(100)]
    public string? Nationality { get; set; }

    public DateTimeOffset? BirthDate { get; set; }

    public DateTimeOffset? DeathDate { get; set; }

    [MaxLength(255)]
    public string? Website { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    // Navigation property (optional, if needed)
    public ICollection<Book>? Books { get; set; }
}
