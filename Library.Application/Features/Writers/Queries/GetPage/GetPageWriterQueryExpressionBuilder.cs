using System.Linq.Expressions;
using Library.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Features.Writers.Queries.GetPage;

internal static class GetPageWriterQueryExpressionBuilder
{
    internal static Expression<Func<Writer, bool>> BuildFilter(GetPageWriterQuery request)
    {
        Expression<Func<Writer, bool>> predicate = w => true;

        if (!string.IsNullOrWhiteSpace(request.FirstName))
            predicate = predicate.And(x =>
                EF.Functions.Like(x.FirstName.ToLower(), $"%{request.FirstName.ToLower()}%"));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            predicate = predicate.And(x => x.LastName != null &&
                                           EF.Functions.Like(x.LastName.ToLower(), $"%{request.LastName.ToLower()}%"));

        if (!string.IsNullOrWhiteSpace(request.Nationality))
            predicate = predicate.And(w => w.Nationality == request.Nationality);

        if (request.BirthDateFrom.HasValue)
            predicate = predicate.And(w => w.BirthDate >= request.BirthDateFrom.Value);

        if (request.BirthDateTo.HasValue)
            predicate = predicate.And(w => w.BirthDate <= request.BirthDateTo.Value);

        if (!request.IsDead.HasValue) return predicate;
        {
            predicate = request.IsDead.Value
                ? predicate.And(w => w.DeathDate != null)
                : predicate.And(w => w.DeathDate == null);
        }

        return predicate;
    }

    internal static Expression<Func<Writer, object>> BuildOrderBy(string? fieldName)
    {
        return fieldName?.ToLowerInvariant() switch
        {
            "firstname" => x => x.FirstName,
            "lastname" => x => x.LastName ?? string.Empty,
            "nationality" => x => x.Nationality ?? string.Empty,
            "birthdate" => x => x.BirthDate ?? DateTimeOffset.MinValue,
            "deathdate" => x => x.DeathDate ?? DateTimeOffset.MaxValue,
            "website" => x => x.Website ?? string.Empty,
            "email" => x => x.Email ?? string.Empty,
            "id" => x => x.Id,
            _ => x => x.Id // varsayılan: ID ile sırala
        };
    }
}