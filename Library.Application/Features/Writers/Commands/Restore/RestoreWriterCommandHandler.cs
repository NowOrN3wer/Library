using Library.Application.Common.Interfaces;
using Library.Application.Features.Writers.Commands.Update;
using Library.Domain.Enums;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Restore;

internal sealed class RestoreWriterCommandHandler(
    IWriterRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<RestoreWriterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(RestoreWriterCommand request, CancellationToken cancellationToken)
    {
        var validator = new RestoreWriterCommandDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsSuccessful || validationResult.Data is null)
        {
            return Result<bool>.Failure(validationResult.ErrorMessages ?? []);
        }

        var writer = validationResult.Data;
        writer.IsDeleted = EntityStatus.ACTIVE;
        repository.Update(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}