using System.Linq.Expressions;
using Library.Application.Dto.Shared;

namespace Library.Application.Common.Interfaces;

public interface ILookupService
{
    Task<LookupResponseDto<TKey>> ForAsync<TEntity, TKey>(
        string? q,
        int limit,
        IEnumerable<TKey>? includeIds,
        string? cursor,
        Expression<Func<TEntity, TKey>> idSelector,
        Expression<Func<TEntity, string>> textSelector,
        Expression<Func<TEntity, bool>>? baseFilter = null,
        CancellationToken ct = default)
        where TEntity : class;
}