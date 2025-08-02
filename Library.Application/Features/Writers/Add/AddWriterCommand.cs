using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Add;

public sealed record AddWriterCommand(
    string firstName,
    string? lastName,
    string? biography,
    string? nationality,
    DateTimeOffset? birthDate,
    DateTimeOffset? deathDate,
    string? website,
    string? email) : IRequest<Result<bool>>;
