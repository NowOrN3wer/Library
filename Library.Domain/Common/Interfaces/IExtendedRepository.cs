using System.Linq.Expressions;
using GenericRepository;

namespace Library.Domain.Common.Interfaces;

public interface IExtendedRepository<T> : IRepository<T> where T : class
{
    Task<(IEnumerable<T> items, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        bool getAllData = false,
        params Expression<Func<T, object>>[] includes);

    Task<(IEnumerable<TResult> items, int totalCount)> GetPagedAsync<TResult>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        bool getAllData = false,
        Expression<Func<T, TResult>> selector = null!,
        CancellationToken cancellationToken = default);
}