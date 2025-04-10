using System.Linq.Expressions;
using GenericRepository;

namespace Library.Domain.Common;

public interface IPagedRepository<T> : IRepository<T> where T : class
{
    Task<(IEnumerable<T> items, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false);
}