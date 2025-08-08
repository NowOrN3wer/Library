using Library.Domain.Abstractions;
using Library.Domain.Common.Interfaces;
using TS.Result;

namespace Library.Application.Common.Validators;

public abstract class BaseEntityByIdValidator<T>(IExtendedRepository<T> repository)
    where T : Entity
{
    protected readonly IExtendedRepository<T> Repo = repository;

    protected async Task<Result<T>> FindAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Repo.GetByExpressionAsync(x => x.Id == id, ct);
        if (entity is not null) return Result<T>.Succeed(entity);
        return Result<T>.Failure([$"{typeof(T).Name} with ID {id} does not exist."]);
    }
}