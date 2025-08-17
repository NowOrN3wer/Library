using System.Linq.Expressions;
using Library.Application.Common.Interfaces;
using Library.Application.Dto.Shared;
using Library.Domain.Entities;
using Library.Domain.Enums;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetLookup;

internal sealed class GetWriterLookupQueryHandler(ILookupService lookup)
    : IRequestHandler<GetWriterLookupQuery, Result<LookupResponseDto<Guid>>>
{
    public async Task<Result<LookupResponseDto<Guid>>> Handle(
        GetWriterLookupQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Writer, Guid>> idSel   = w => w.Id;
        Expression<Func<Writer, string>> txtSel =
            w => (w.FirstName ?? "") + " " + (w.LastName ?? "");
        Expression<Func<Writer, bool>> baseFilter =
            w => w.IsDeleted != EntityStatus.DELETED;

        var resp = await lookup.ForAsync<Writer, Guid>(
            q:          request.Q,
            limit:      request.Limit,
            includeIds: request.IncludeIds,
            cursor:     request.Cursor,        // 👈 cursor eklendi
            idSelector: idSel,
            textSelector: txtSel,
            baseFilter: baseFilter,
            ct:         cancellationToken);

        return Result<LookupResponseDto<Guid>>.Succeed(resp);
    }
}