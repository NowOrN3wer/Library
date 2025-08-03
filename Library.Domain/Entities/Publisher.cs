using Library.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entities;

public sealed class Publisher : Entity
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Website { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    // Navigation: Bu yayınevine ait kitaplar
    public ICollection<Book>? Books { get; set; }
}
