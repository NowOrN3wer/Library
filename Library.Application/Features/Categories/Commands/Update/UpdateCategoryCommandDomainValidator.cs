using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Categories.Commands.Update;

internal sealed class UpdateCategoryCommandDomainValidator(ICategoryRepository repository)
    : BaseActiveEntityByIdValidator<Category, UpdateCategoryCommand>(repository)
{
    public async Task<Result<Category>> ValidateAsync(UpdateCategoryCommand request)
    {
        return await ValidateAsync(request, request.Id);
    }
}