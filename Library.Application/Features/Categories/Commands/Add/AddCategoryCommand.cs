using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Add;

public sealed record AddCategoryCommand(string Name, string? Description) : IRequest<Result<bool>>;
