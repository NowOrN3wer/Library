using Library.Application.Common.Interfaces;
using Library.Domain.Enums;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Delete;

internal sealed class DeleteCategoryCommandHandler(
    ICategoryRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteCategoryCommandDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsSuccessful || validationResult.Data is null)
        {
            return Result<bool>.Failure(validationResult.ErrorMessages ?? []);
        }

        var writer = validationResult.Data;
        writer.IsDeleted = EntityStatus.DELETED;
        repository.Update(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}