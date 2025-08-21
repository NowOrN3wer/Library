using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Add;

internal sealed class AddCategoryCommandHandler(
    ICategoryRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<AddCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Category>();
        await repository.AddAsync(entity, cancellationToken);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}