using Library.Application.Dto.Shared;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetLookup;

public sealed record GetWriterLookupQuery : LookupRequestDto<Guid>, IRequest<Result<LookupResponseDto<Guid>>>;