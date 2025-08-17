using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Result<bool>>;
