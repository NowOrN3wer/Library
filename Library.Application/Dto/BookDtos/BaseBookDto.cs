using Library.Application.Dto.Abstractions;

namespace Library.Application.Dto.BookDtos;

public abstract record BaseBookDto(
    string Title,
    string? Summary,
    string? ISBN,
    string? Language,
    int? PageCount,
    DateTimeOffset? PublishedDate
) : EntityDto;