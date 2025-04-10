using GenericRepository;
using TS.Result;

namespace Library.Application.Common.Interfaces;

public interface IUnitOfWorkWithTransaction : IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
    Task<bool> SaveChangesAndReturnSuccessAsync(CancellationToken cancellationToken = default);
    Task<Result<bool>> SaveChangesAsResultAsync(CancellationToken cancellationToken = default);
}