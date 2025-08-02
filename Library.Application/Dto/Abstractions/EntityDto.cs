using Library.Domain.Enums;

namespace Library.Application.Dto.Abstractions;

public abstract record EntityDto
{
    public Guid id { get; init; }
    public int version { get; init; }
    public string? createdBy { get; init; }
    public string? updatedBy { get; init; }
    public DateTimeOffset createdAt { get; init; }
    public DateTimeOffset? updatedAt { get; init; }
    public EntityStatus isDeleted { get; init; }
}
