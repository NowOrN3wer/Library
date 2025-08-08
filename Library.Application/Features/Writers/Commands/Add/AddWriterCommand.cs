using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Add;

public sealed record AddWriterCommand : IRequest<Result<bool>>
{
    public required string FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Biography { get; init; }
    public string? Nationality { get; init; }
    public DateTimeOffset? BirthDate { get; init; }
    public DateTimeOffset? DeathDate { get; init; }
    public string? Website { get; init; }
    public string? Email { get; init; }
}