using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Add;

public sealed record class AddWriterCommand : IRequest<Result<bool>>
{
    public string firstName { get; init; } = default!;
    public string? lastName { get; init; }
    public string? biography { get; init; }
    public string? nationality { get; init; }
    public DateTimeOffset? birthDate { get; init; }
    public DateTimeOffset? deathDate { get; init; }
    public string? website { get; init; }
    public string? email { get; init; }
}
