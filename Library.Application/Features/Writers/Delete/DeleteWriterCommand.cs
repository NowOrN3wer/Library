using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Delete;

public sealed record DeleteWriterCommand(Guid Id) : IRequest<Result<bool>>;