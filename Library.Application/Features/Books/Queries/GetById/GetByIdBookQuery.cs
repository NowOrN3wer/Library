using Library.Application.Dto.BookDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Queries.GetById;

public sealed record GetByIdBookQuery(Guid Id) : IRequest<Result<BookDetailDto>>;