using Library.Application.Common.Interfaces;
using Library.Domain.Enums;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Delete;

internal sealed class DeleteWriterCommandHandler(
    IWriterRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<DeleteWriterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteWriterCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteWriterCommandDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsSuccessful || validationResult.Data is null)
            return Result<bool>.Failure(validationResult.ErrorMessages ?? []);

        var writer = validationResult.Data;
        writer.IsDeleted = EntityStatus.DELETED;
        repository.Update(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}