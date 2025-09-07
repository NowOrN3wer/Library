using System.ComponentModel.DataAnnotations;
using Library.Domain.Abstractions;

namespace Library.Domain.Entities;

public sealed class Category : Entity
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string? Description { get; set; }

    // Navigation: Bu kategoriye ait kitaplar
    public ICollection<Book>? Books { get; set; }
}