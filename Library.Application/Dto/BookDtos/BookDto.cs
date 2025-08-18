namespace Library.Application.Dto.BookDtos;

public sealed record BookDto(
    Guid WriterId,
    string WriterName,
    Guid CategoryId,
    string CategoryName,
    Guid PublisherId,
    string PublisherName,
    string Title,
    string? Summary,
    string? ISBN,
    string? Language,
    int? PageCount,
    DateTimeOffset? PublishedDate
) : BaseBookDto(Title, Summary, ISBN, Language, PageCount, PublishedDate);