using MediatR;
using TS.Result;


namespace Library.Application.Features.Publishers.Commands.Add;

public sealed record AddPublisherCommand : IRequest<Result<bool>>
{
    public required string Name { get; init; }
    public string? Website { get; init; }
    public string? Address { get; init; }
    public string? Country { get; init; }
}