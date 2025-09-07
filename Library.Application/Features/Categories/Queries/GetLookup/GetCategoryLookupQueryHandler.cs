using System.Linq.Expressions;
using Library.Application.Common.Interfaces;
using Library.Application.Dto.Shared;
using Library.Domain.Entities;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Queries.GetLookup;

internal sealed class GetCategoryLookupQueryHandler(ILookupService lookup)
    : IRequestHandler<GetCategoryLookupQuery, Result<LookupResponseDto<Guid>>>
{
    public async Task<Result<LookupResponseDto<Guid>>> Handle(
        GetCategoryLookupQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Category, Guid>> idSel = c => c.Id;
        Expression<Func<Category, string>> txtSel =
            w => w.Name ?? "";

        var resp = await lookup.ForAsync<Category, Guid>(
            request.Q,
            request.Limit,
            request.IncludeIds,
            request.Cursor,
            idSel,
            txtSel,
            null,
            cancellationToken);

        return Result<LookupResponseDto<Guid>>.Succeed(resp);
    }
}