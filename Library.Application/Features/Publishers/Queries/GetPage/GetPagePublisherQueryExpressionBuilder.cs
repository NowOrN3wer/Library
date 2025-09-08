using System.Linq.Expressions;
using Library.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Features.Publishers.Queries.GetPage;

internal static class GetPagePublisherQueryExpressionBuilder
{
    internal static Expression<Func<Publisher, bool>> BuildFilter(GetPagePublisherQuery request)
    {
        Expression<Func<Publisher, bool>> predicate = w => true;

        if (!string.IsNullOrWhiteSpace(request.Name))
            predicate = predicate.And(x =>
                EF.Functions.ILike(x.Name, $"%{request.Name}%"));

        if (!string.IsNullOrWhiteSpace(request.Website))
            predicate = predicate.And(x => x.Website != null &&
                                           EF.Functions.ILike(x.Website,
                                               $"%{request.Website}%"));

        if (!string.IsNullOrWhiteSpace(request.Address))
            predicate = predicate.And(x => x.Address != null &&
                                           EF.Functions.ILike(x.Address,
                                               $"%{request.Address}%"));

        if (!string.IsNullOrWhiteSpace(request.Country))
            predicate = predicate.And(x => x.Country != null &&
                                           EF.Functions.ILike(x.Country,
                                               $"%{request.Country}%"));

        return predicate;
    }

    internal static Expression<Func<Publisher, object>> BuildOrderBy(string? fieldName)
    {
        return fieldName?.ToLowerInvariant() switch
        {
            "name" => x => x.Name,
            "address" => x => x.Address ?? string.Empty,
            "website" => x => x.Website ?? string.Empty,
            "country" => x => x.Country ?? string.Empty,
            "id" => x => x.Id,
            _ => x => x.Id
        };
    }
}