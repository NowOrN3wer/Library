using System.Linq.Expressions;
using Library.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Features.Categories.Queries.GetPage;

internal static class GetPageCategoryQueryExpressionBuilder
{
    internal static Expression<Func<Category, bool>> BuildFilter(GetPageCategoryQuery request)
    {
        Expression<Func<Category, bool>> predicate = w => true;

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            predicate = predicate.And(x =>
                EF.Functions.ILike(x.Name, $"%{request.Name}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            predicate = predicate.And(x => x.Description != null &&
                                           EF.Functions.Like(x.Description.ToLower(),
                                               $"%{request.Description.ToLower()}%"));
        }

        return predicate;
    }

    internal static Expression<Func<Category, object>> BuildOrderBy(string? fieldName)
    {
        return fieldName?.ToLowerInvariant() switch
        {
            "name" => x => x.Name,
            "description" => x => x.Description ?? string.Empty,
            "id" => x => x.Id,
            _ => x => x.Id
        };
    }
}