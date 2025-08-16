using Library.Application.Dto.CommonDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Queries.GetLookupQuery;

public sealed record GetCategoryLookupQuery()
    : LookupRequestDto<Guid>, IRequest<Result<IReadOnlyList<LookupItemDto<Guid>>>>;