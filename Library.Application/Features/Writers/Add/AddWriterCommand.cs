using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Add;

public sealed record class AddWriterCommand : IRequest<Result<bool>>
{
    public string FirstName { get; init; } = default!;
    public string? LastName { get; init; }
    public string? Biography { get; init; }
    public string? Nationality { get; init; }
    public DateTimeOffset? BirthDate { get; init; }
    public DateTimeOffset? DeathDate { get; init; }
    public string? Website { get; init; }
    public string? Email { get; init; }
}
