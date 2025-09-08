using Library.Application.Dto.Shared;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Queries.GetLookup;

public sealed record GetCategoryLookupQuery : LookupRequestDto<Guid>, IRequest<Result<LookupResponseDto<Guid>>>;