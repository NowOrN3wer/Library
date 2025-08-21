using Library.Application.Dto.CategoryDtos;
using Library.Application.Dto.PublisherDtos;
using Library.Application.Dto.WriterDtos;

namespace Library.Application.Dto.BookDtos;

public sealed record BookDetailDto(
    WriterDto Writer,
    CategoryDto Category,
    PublisherDto Publisher,
    string Title,
    string? Summary,
    string? ISBN,
    string? Language,
    int? PageCount,
    DateTimeOffset? PublishedDate
) : BaseBookDto(Title, Summary, ISBN, Language, PageCount, PublishedDate);