using Library.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Entities;

public sealed class Book : Entity
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Summary { get; set; }

    [MaxLength(13)]
    public string? ISBN { get; set; }

    [MaxLength(100)]
    public string? Language { get; set; }

    public int? PageCount { get; set; }

    public DateTimeOffset? PublishedDate { get; set; }

    // Writer
    [Required]
    public Guid WriterId { get; set; }

    [ForeignKey(nameof(WriterId))]
    public Writer Writer { get; set; } = default!;

    // Category
    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;

    // Publisher
    [Required]
    public Guid PublisherId { get; set; }

    [ForeignKey(nameof(PublisherId))]
    public Publisher Publisher { get; set; } = default!;
}
