using Library.Application.Dto.Abstractions;
using Library.Application.Dto.BookDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Queries.GetPage;

public sealed record GetPageBookQuery(
    string? WriterName,
    string? CategoryName,
    string? PublisherName,
    string? Title,
    string? Summary,
    string? ISBN,
    string? Language,
    int? PageCount,
    DateTimeOffset? PublishedDate)
    : BasePageRequestDto, IRequest<Result<BasePageResponseDto<BookDto>>>;