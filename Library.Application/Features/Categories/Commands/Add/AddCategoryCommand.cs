using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Add;

public sealed record AddCategoryCommand : IRequest<Result<bool>>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}