using Library.Application.Dto.CommonDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetLookup;

public sealed record GetWriterLookupQuery()
    : LookupRequestDto<Guid>, IRequest<Result<IReadOnlyList<LookupItemDto<Guid>>>>;