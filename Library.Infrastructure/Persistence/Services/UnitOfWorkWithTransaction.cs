using GenericRepository;
using Library.Application.Common.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using TS.Result;

namespace Library.Infrastructure.Persistence.Services;

public class UnitOfWorkWithTransaction(IUnitOfWork innerUnitOfWork, ApplicationDbContext context)
    : IUnitOfWorkWithTransaction
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
    {
        await BeginTransactionAsync();
        try
        {
            var result = await action();
            await CommitTransactionAsync();
            return result;
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return innerUnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return innerUnitOfWork.SaveChanges();
    }

    public async Task<bool> SaveChangesAndReturnSuccessAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Result<bool>> SaveChangesAsResultAsync(CancellationToken cancellationToken = default)
    {
        var success = await SaveChangesAsync(cancellationToken) > 0;
        return success
            ? Result<bool>.Succeed(true)
            : Result<bool>.Failure("Değişiklik kaydedilemedi.");
    }
}