using Library.Domain.Enums;
using Library.Domain.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Abstractions;

public abstract class Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public int Version { get; set; } = 1;

    [Required]
    [MaxLength(255)]
    public string? CreatedBy { get; set; }

    [MaxLength(255)]
    public string? UpdatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = TimeHelper.GetTurkeyTime();

    public DateTimeOffset? UpdatedAt { get; set; }

    public EntityStatus IsDeleted { get; set; } = EntityStatus.ACTIVE;
}