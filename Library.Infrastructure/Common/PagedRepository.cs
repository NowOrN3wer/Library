using System.Linq.Expressions;
using GenericRepository;
using Library.Domain.Common;
using Library.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace Library.Infrastructure.Common;

public class PagedRepository<T>(ApplicationDbContext context)
    : Repository<T, ApplicationDbContext>(context), IPagedRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _context = context;

    public async Task<(IEnumerable<T> items, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false)
    {
        if (pageNumber <= 0) pageNumber = 1;

        var query = _context.Set<T>().AsExpandable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = isDescending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
