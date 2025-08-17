using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Common.Expressions;

public static class QueryExpressionBuilder
{
    /// <summary>
    /// text doluysa, verilen string selector(ler) üzerinde ILIKE '%text%' OR birleştirerek filtre üretir.
    /// Nullable alanları predicate dışı bırakır (x.Prop != null && ILIKE(...)).
    /// text null/boşsa "true" döner (no-op).
    /// </summary>
    public static Expression<Func<T, bool>> BuildContainsFilter<T>(
        string? text,
        params Expression<Func<T, string?>>[] selectors)
    {
        var predicate = PredicateBuilder.New<T>(true);

        if (!string.IsNullOrWhiteSpace(text) && selectors is { Length: > 0 })
        {
            var lowered = text.ToLower();
            var or = PredicateBuilder.New<T>(false);

            foreach (var sel in selectors)
            {
                Expression<Func<T, bool>> part =
                    x => sel.Invoke(x) != null &&
                         EF.Functions.ILike(sel.Invoke(x)!, $"%{lowered}%");

                or = or.Or(part);
            }

            predicate = predicate.And(or);
        }

        return predicate;
    }

    /// <summary>
    /// Alan adına göre tip-güvenli OrderBy seçer. Eşleşme yoksa defaultKey döner.
    /// </summary>
    public static Expression<Func<T, object>> BuildOrderBy<T>(
        string? fieldName,
        IDictionary<string, Expression<Func<T, object>>> map,
        string defaultKey)
    {
        if (!string.IsNullOrWhiteSpace(fieldName))
        {
            var key = fieldName.ToLowerInvariant();
            if (map.TryGetValue(key, out var expr))
                return expr;
        }
        return map[defaultKey];
    }
}