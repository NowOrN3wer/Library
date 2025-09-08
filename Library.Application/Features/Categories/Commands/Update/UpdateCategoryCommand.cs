using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Update;

public sealed record UpdateCategoryCommand : IRequest<Result<bool>>
{
    public required Guid Id { get; set; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}