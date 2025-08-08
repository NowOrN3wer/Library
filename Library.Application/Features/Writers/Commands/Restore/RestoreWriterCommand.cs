using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Restore;

public sealed record RestoreWriterCommand(Guid Id) : IRequest<Result<bool>>;