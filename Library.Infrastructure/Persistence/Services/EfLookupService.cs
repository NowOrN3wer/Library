using Library.Application.Common.Interfaces;
using Library.Application.Dto.CommonDtos;
using Library.Infrastructure.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace Library.Infrastructure.Persistence.Services;

public sealed class EfLookupService(ApplicationDbContext db) : ILookupService
{
    public async Task<IReadOnlyList<LookupItemDto<TKey>>> ForAsync<TEntity, TKey>(
        string? q,
        int limit,
        IEnumerable<TKey>? includeIds,
        Expression<Func<TEntity, TKey>> idSelector,
        Expression<Func<TEntity, string>> textSelector,
        Expression<Func<TEntity, bool>>? baseFilter = null,
        CancellationToken ct = default)
        where TEntity : class
    {
        var set = db.Set<TEntity>()
                    .AsNoTracking()
                    .AsExpandable();

        if (baseFilter is not null)
            set = set.Where(baseFilter);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            var isPg = db.Database.ProviderName?.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0;

            if (isPg)
            {
                set = set.Where(e => EF.Functions.ILike(textSelector.Invoke(e), $"%{term}%"));
            }
            else
            {
                var lower = term.ToLowerInvariant();
                set = set.Where(e => EF.Functions.Like(textSelector.Invoke(e).ToLower(), $"%{lower}%"));
            }
        }

        var isPg2 = db.Database.ProviderName?.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0;
        if (isPg2)
            set = set.OrderBy(e => EF.Functions.Collate(textSelector.Invoke(e), "tr-x-icu"));
        else
            set = set.OrderBy(textSelector);

        var take = limit > 0 ? limit : 20;

        var list = await set
            .Select(e => new LookupItemDto<TKey>(
                idSelector.Invoke(e),
                textSelector.Invoke(e)))
            .Take(take)
            .ToListAsync(ct);

        if (includeIds is { } ids && ids.Any())
        {
            var missing = ids.Except(list.Select(x => x.Id)).ToArray();
            if (missing.Length > 0)
            {
                var extrasQuery = db.Set<TEntity>()
                                    .AsNoTracking()
                                    .AsExpandable();

                if (baseFilter is not null)
                    extrasQuery = extrasQuery.Where(baseFilter);

                var extras = await extrasQuery
                    .Where(e => missing.Contains(idSelector.Invoke(e)))
                    .Select(e => new LookupItemDto<TKey>(
                        idSelector.Invoke(e),
                        textSelector.Invoke(e)))
                    .ToListAsync(ct);

                list = [.. list
                    .Concat(extras)
                    .DistinctBy(x => x.Id)];
            }
        }

        list = [.. list.OrderBy(x => x.Text, StringComparer.Create(new CultureInfo("tr-TR"), ignoreCase: false))];

        return list;
    }
}
