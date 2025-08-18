using Library.Application.Dto.Abstractions;
using Library.Application.Dto.PublisherDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Publishers.Queries.GetPage;

public sealed record GetPagePublisherQuery(
    string? Name,
    string? Website,
    string? Address,
    string? Country)
    : BasePageRequestDto, IRequest<Result<BasePageResponseDto<PublisherDto>>>;
    