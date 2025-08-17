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
            w => (w.Name ?? "");

        var resp = await lookup.ForAsync<Category, Guid>(
            q:          request.Q,
            limit:      request.Limit,
            includeIds: request.IncludeIds,
            cursor:     request.Cursor,
            idSelector: idSel,
            textSelector: txtSel,
            baseFilter: null,
            ct:         cancellationToken);

        return Result<LookupResponseDto<Guid>>.Succeed(resp);
    }
}