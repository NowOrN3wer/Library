using Library.Application.Common.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using TS.Result;

namespace Library.Infrastructure.Persistence.Services;

public class UnitOfWorkWithTransaction(ApplicationDbContext context)
    : IUnitOfWorkWithTransaction
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null) return;
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;
        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await action(cancellationToken);
            await CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public int SaveChanges()
        => context.SaveChanges();

    public async Task<bool> SaveChangesAndReturnSuccessAsync(CancellationToken cancellationToken = default)
        => await SaveChangesAsync(cancellationToken) > 0;

    public async Task<Result<bool>> SaveChangesAsResultAsync(CancellationToken cancellationToken = default)
        => (await SaveChangesAsync(cancellationToken) > 0)
            ? Result<bool>.Succeed(true)
            : Result<bool>.Failure("Değişiklik kaydedilemedi.");
}
