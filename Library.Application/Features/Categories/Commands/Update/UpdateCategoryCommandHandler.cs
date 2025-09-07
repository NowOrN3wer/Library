using Library.Application.Common.Interfaces;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Update;

internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCategoryCommandDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsSuccessful || validationResult.Data is null)
            return Result<bool>.Failure(validationResult.ErrorMessages ?? []);

        var writer = validationResult.Data;

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != writer.Name)
            writer.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description != writer.Description)
            writer.Description = request.Description;

        repository.Update(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}