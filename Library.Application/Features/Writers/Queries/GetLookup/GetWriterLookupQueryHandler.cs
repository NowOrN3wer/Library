using Library.Application.Common.Interfaces;
using Library.Application.Dto.CommonDtos;
using Library.Domain.Entities;
using Library.Domain.Enums;
using MediatR;
using System.Linq.Expressions;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetLookup;

internal class GetWriterLookupQueryHandler(ILookupService lookup) : IRequestHandler<GetWriterLookupQuery, Result<IReadOnlyList<LookupItemDto<Guid>>>>
{
    public async Task<Result<IReadOnlyList<LookupItemDto<Guid>>>> Handle(
        GetWriterLookupQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Writer, Guid>> idSel = w => w.Id;
        Expression<Func<Writer, string>> textSel =
            w => (w.FirstName ?? "") + " " + (w.LastName ?? "");
        Expression<Func<Writer, bool>> baseFilter = w  => w.IsDeleted != EntityStatus.DELETED;

        var items = await lookup.ForAsync<Writer, Guid>(
            q: request.Q,
            limit: request.Limit,
            includeIds: request.IncludeIds ?? [],
            idSelector: idSel,
            textSelector: textSel,
            baseFilter: baseFilter,
            ct: cancellationToken);

        return Result<IReadOnlyList<LookupItemDto<Guid>>>.Succeed(
            items ?? []
        );
    }
}