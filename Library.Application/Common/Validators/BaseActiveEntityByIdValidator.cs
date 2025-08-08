using Library.Domain.Abstractions;
using Library.Domain.Common.Interfaces;
using Library.Domain.Enums;
using TS.Result;

namespace Library.Application.Common.Validators;

public abstract class BaseActiveEntityByIdValidator<T, TRequest>(IExtendedRepository<T> repository)
    where T : Entity
{
    protected async Task<Result<T>> ValidateAsync(TRequest request, Guid id)
    {
        var errors = new List<string>();

        var entity = await repository.GetByExpressionAsync(x =>
            x.Id.Equals(id) && x.IsDeleted == EntityStatus.ACTIVE);

        if (entity is not null)
            return Result<T>.Succeed(entity);

        errors.Add($"{typeof(T).Name} with ID {id} does not exist or is deleted.");
        return Result<T>.Failure(errors);
    }
}