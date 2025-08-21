namespace Library.Application.Dto.Abstractions;

public abstract record EntityDto
{
    public Guid Id { get; init; }
    public int Version { get; init; }
    public string? CreatedBy { get; init; }
    public string? UpdatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}
