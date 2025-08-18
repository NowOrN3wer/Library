using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Commands.Add;

public sealed record AddBookCommand(
    string Title,
    string? Summary,
    string? ISBN,
    string? Language,
    int? PageCount,
    DateTimeOffset? PublishedDate,
    Guid WriterId,
    Guid CategoryId,
    Guid PublisherId
) : IRequest<Result<bool>>;