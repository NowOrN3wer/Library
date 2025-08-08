using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Delete;

public sealed record DeleteWriterCommand(Guid Id) : IRequest<Result<bool>>;