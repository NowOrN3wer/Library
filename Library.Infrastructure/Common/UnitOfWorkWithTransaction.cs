using GenericRepository;
using Library.Application.Common.Interfaces;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using TS.Result;

namespace Library.Infrastructure.Common;

public class UnitOfWorkWithTransaction : IUnitOfWorkWithTransaction
{
    private readonly IUnitOfWork _innerUnitOfWork;
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWorkWithTransaction(IUnitOfWork innerUnitOfWork, ApplicationDbContext context)
    {
        _innerUnitOfWork = innerUnitOfWork;
        _context = context;
    }

    public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

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

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _innerUnitOfWork.SaveChangesAsync(cancellationToken);

    public int SaveChanges() => _innerUnitOfWork.SaveChanges();

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