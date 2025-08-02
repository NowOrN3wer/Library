using System.Linq.Expressions;
using GenericRepository;
using Library.Domain.Common.Interfaces;
using Library.Infrastructure.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence.Repositories;

public class ExtendedRepository<T>(ApplicationDbContext context)
    : Repository<T, ApplicationDbContext>(context), IExtendedRepository<T>
    where T : class
{
    private readonly ApplicationDbContext _context = context;

    public async Task<(IEnumerable<T> items, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        bool getAllData = false,
        params Expression<Func<T, object>>[] includes)
    {
        if (pageNumber <= 0) pageNumber = 1;

        var query = _context.Set<T>().AsExpandable();

        if (filter is not null) query = query.Where(filter);

        if (includes is not null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (orderBy is not null)
            query = isDescending
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
        
        var totalCount = await query.CountAsync();

        if (getAllData)
        {
            var allItems = await query.ToListAsync();
            return (allItems, totalCount);
        }


        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}