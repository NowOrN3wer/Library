using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Delete;

internal sealed class DeleteCategoryCommandDomainValidator(ICategoryRepository repository)
    : BaseActiveEntityByIdValidator<Category, DeleteCategoryCommand>(repository)
{
    public async Task<Result<Category>> ValidateAsync(DeleteCategoryCommand request)
    {
        return await ValidateAsync(request, request.Id);
    }
}