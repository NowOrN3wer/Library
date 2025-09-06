using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Add;

public sealed class AddWriterCommandHandler(
    IWriterRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<AddWriterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddWriterCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Writer>();
        await repository.AddAsync(entity, cancellationToken);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}