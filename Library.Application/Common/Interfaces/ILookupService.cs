using Library.Application.Dto.CommonDtos;
using System.Linq.Expressions;

namespace Library.Application.Common.Interfaces;

public interface ILookupService
{
    Task<IReadOnlyList<LookupItemDto<TKey>>> ForAsync<TEntity, TKey>(
        string? q,
        int limit,
        IEnumerable<TKey>? includeIds,
        Expression<Func<TEntity, TKey>> idSelector,
        Expression<Func<TEntity, string>> textSelector,
        Expression<Func<TEntity, bool>>? baseFilter = null,
        CancellationToken ct = default)
        where TEntity : class;
}
