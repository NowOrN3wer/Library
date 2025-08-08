using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Update;

public sealed record UpdateWriterCommand : IRequest<Result<bool>>
{
    public required Guid Id { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Biography { get; init; }
    public string? Nationality { get; init; }
    public DateTimeOffset? BirthDate { get; init; }
    public DateTimeOffset? DeathDate { get; init; }
    public string? Website { get; init; }
    public string? Email { get; init; }
}