using Library.Application.Dto.CommonDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetLookup;

public sealed record GetWriterLookupQuery(
    string? Q,
    int Limit = 20,
    List<Guid>? IncludeIds = null
) : IRequest<Result<IReadOnlyList<LookupItemDto<Guid>>>>;
