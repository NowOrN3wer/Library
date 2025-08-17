using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.Common.Interfaces;
using Library.Application.Dto.Shared;
using Library.Domain.Abstractions;      // Entity
using Library.Domain.Enums;             // EntityStatus
using Library.Infrastructure.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL; // EF.Functions.ILike

namespace Library.Infrastructure.Persistence.Services;

public sealed class EfLookupService(ApplicationDbContext db) : ILookupService
{
    // DI taramasına takılmaması için value type:
    private readonly record struct CursorDto(string Text, Guid Id);

    public async Task<LookupResponseDto<TKey>> ForAsync<TEntity, TKey>(
        string? q,
        int limit,
        System.Collections.Generic.IEnumerable<TKey>? includeIds,
        string? cursor,
        Expression<Func<TEntity, TKey>> idSelector,
        Expression<Func<TEntity, string>> textSelector,
        Expression<Func<TEntity, bool>>? baseFilter = null,
        CancellationToken ct = default)
        where TEntity : class
    {
        // Guards
        var idSel   = idSelector  ?? throw new ArgumentNullException(nameof(idSelector));
        var textSel = textSelector ?? throw new ArgumentNullException(nameof(textSelector));

        limit = Math.Clamp(limit <= 0 ? 20 : limit, 1, 100);

        IQueryable<TEntity> set = db.Set<TEntity>()
            .AsNoTracking()
            .AsExpandable();

        // ★ Otomatik ACTIVE filtresi (TEntity, Entity'den türemişse)
        if (typeof(Entity).IsAssignableFrom(typeof(TEntity)))
        {
            var p   = Expression.Parameter(typeof(TEntity), "e");
            var prop= Expression.Property(p, nameof(Entity.IsDeleted));              // e.IsDeleted
            var val = Expression.Constant(EntityStatus.ACTIVE);                      // ACTIVE
            var eq  = Expression.Equal(prop, val);                                   // e.IsDeleted == ACTIVE
            var activeLambda = Expression.Lambda<Func<TEntity, bool>>(eq, p);
            set = set.Where(activeLambda);
        }

        // İsteğe bağlı ek filtre
        if (baseFilter is not null)
            set = set.Where(baseFilter);

        // Arama
        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            var isPg = db.Database.ProviderName?.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0;

            if (isPg)
                set = set.Where(e => EF.Functions.ILike(textSel.Invoke(e), $"%{term}%"));
            else
            {
                var lower = term.ToLowerInvariant();
                set = set.Where(e => EF.Functions.Like(textSel.Invoke(e).ToLower(), $"%{lower}%"));
            }
        }

        // Projeksiyon (Text + Id + IdStr) — IdStr seek tie-break için
        var isPg2 = db.Database.ProviderName?.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0;

        var collated = isPg2
            ? set.Select(e => new
              {
                  Id    = idSel.Invoke(e),
                  IdStr = idSel.Invoke(e) == null ? string.Empty : idSel.Invoke(e)!.ToString(),
                  Text  = EF.Functions.Collate(textSel.Invoke(e), "tr-x-icu")
              })
            : set.Select(e => new
              {
                  Id    = idSel.Invoke(e),
                  IdStr = idSel.Invoke(e) == null ? string.Empty : idSel.Invoke(e)!.ToString(),
                  Text  = textSel.Invoke(e)
              });

        // Cursor decode (Guid)
        string? afterText = null;
        Guid? afterGuid = null;

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
                if (typeof(TKey) == typeof(Guid))
                {
                    CursorDto? dto = JsonSerializer.Deserialize<CursorDto?>(json);
                    if (dto.HasValue && !string.IsNullOrEmpty(dto.Value.Text) && dto.Value.Id != Guid.Empty)
                    {
                        afterText = dto.Value.Text;
                        afterGuid = dto.Value.Id;
                    }
                }
            }
            catch { /* bozuk cursor'u yok say */ }
        }

        // Seek (Text, IdStr) — 2 parametreli Compare kullan
        if (afterText is not null && typeof(TKey) == typeof(Guid))
        {
            var afterIdStr = afterGuid!.Value.ToString();

            collated = collated.Where(x =>
                string.Compare(x.Text, afterText) > 0 ||
                (x.Text == afterText && string.Compare(x.IdStr, afterIdStr) > 0));
        }

        // Sayfa (limit + 1 → hasMore)
        var page = await collated
            .OrderBy(x => x.Text)
            .ThenBy(x => x.Id)
            .Take(limit + 1)
            .Select(x => new { x.Id, x.Text })
            .ToListAsync(ct);

        var hasMore = page.Count > limit;
        if (hasMore) page.RemoveAt(page.Count - 1);

        // Items
        var items = page
            .Select(x => new LookupItemDto<TKey>((TKey)(object)x.Id!, x.Text))
            .ToList();

        // includeIds: seçili olanları garanti et
        if (includeIds is { } ids && ids.Any())
        {
            var missing = ids.Except(items.Select(i => i.Id)).ToArray();
            if (missing.Length > 0)
            {
                var extras = await db.Set<TEntity>()
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(e => missing.Contains(idSel.Invoke(e)))
                    .Select(e => new LookupItemDto<TKey>(idSel.Invoke(e), textSel.Invoke(e)))
                    .ToListAsync(ct);

                items = items.Concat(extras).DistinctBy(i => i.Id).ToList();
            }
        }

        // Son sıralama (tr-TR)
        items = items.OrderBy(i => i.Text,
                 StringComparer.Create(new CultureInfo("tr-TR"), ignoreCase: false)).ToList();

        // NextCursor (Guid)
        string? nextCursor = null;
        if (hasMore && items.Count > 0 && typeof(TKey) == typeof(Guid))
        {
            var last = page[^1];
            var dto  = new CursorDto(last.Text, (Guid)(object)last.Id!);
            nextCursor = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dto)));
        }

        return new LookupResponseDto<TKey>(items, nextCursor, hasMore);
    }
}
