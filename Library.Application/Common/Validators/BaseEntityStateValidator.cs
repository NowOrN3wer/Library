using Library.Domain.Abstractions;
using Library.Domain.Common.Interfaces;
using Library.Domain.Enums;
using TS.Result;

namespace Library.Application.Common.Validators;

public abstract class BaseEntityStateValidator<T> : BaseEntityByIdValidator<T>
    where T : Entity
{
    protected BaseEntityStateValidator(IExtendedRepository<T> repository) : base(repository) { }

    protected async Task<Result<T>> EnsureStateAsync(
        Guid id,
        EntityStatus expected,
        string mismatchMessage,
        CancellationToken ct = default)
    {
        var entity = await Repo.GetByExpressionAsync(x => x.Id == id, ct);
        if (entity is null)
            return Result<T>.Failure([$"{typeof(T).Name} with ID {id} does not exist."]);

        if (entity.IsDeleted != expected)
            return Result<T>.Failure([mismatchMessage]);

        return Result<T>.Succeed(entity);
    }
}