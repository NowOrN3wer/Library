using Library.Application.Dto.BookDtos;
using Library.Application.Dto.CategoryDtos;
using Library.Application.Dto.PublisherDtos;
using Library.Application.Dto.WriterDtos;
using Library.Application.Features.Books.Commands.Add;
using Library.Domain.Entities;
using Mapster;

namespace Library.Application.Mapping.BookMapping;

public class BookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // ---------------------------
        // Entity -> BookDto (flat)
        // BookDto ctor imzası:
        // (Guid writerId, string writerName, Guid categoryId, string categoryName,
        //  Guid publisherId, string publisherName,
        //  string title, string? summary, string? isbn, string? language, int? pageCount, DateTimeOffset? publishedDate)
        // ---------------------------
        config.NewConfig<Book, BookDto>()
            .ConstructUsing(src => new BookDto(
                src.WriterId,
                src.Writer != null ? src.Writer.FullName : string.Empty, // WriterName
                src.CategoryId,
                src.Category != null ? src.Category.Name : string.Empty, // CategoryName
                src.PublisherId,
                src.Publisher != null ? src.Publisher.Name : string.Empty, // PublisherName
                src.Title,
                src.Summary,
                src.ISBN,
                src.Language,
                src.PageCount,
                src.PublishedDate
            ))
            // Base EntityDto alanları
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

        // --------------------------------
        // Entity -> BookDetailDto (nested)
        // BookDetailDto ctor imzası örn:
        // (WriterDto writer, CategoryDto category, PublisherDto publisher,
        //  string title, string? summary, string? isbn, string? language, int? pageCount, DateTimeOffset? publishedDate)
        // --------------------------------
        config.NewConfig<Book, BookDetailDto>()
            .ConstructUsing(src => new BookDetailDto(
                src.Writer == null ? default! : src.Writer.Adapt<WriterDto>(),
                src.Category == null ? default! : src.Category.Adapt<CategoryDto>(),
                src.Publisher == null ? default! : src.Publisher.Adapt<PublisherDto>(),
                src.Title,
                src.Summary,
                src.ISBN,
                src.Language,
                src.PageCount,
                src.PublishedDate
            ))
            // Base EntityDto alanları
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy);

        // ---------------------------
        // BookDto -> Entity
        // ---------------------------
        config.NewConfig<BookDto, Book>()
            // Kimlik & audit alanları
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Version, src => src.Version)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.CreatedBy, src => src.CreatedBy)
            .Map(dest => dest.UpdatedBy, src => src.UpdatedBy)

            // Asıl alanlar
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Summary, src => src.Summary)
            .Map(dest => dest.ISBN, src => src.ISBN)
            .Map(dest => dest.Language, src => src.Language)
            .Map(dest => dest.PageCount, src => src.PageCount)
            .Map(dest => dest.PublishedDate, src => src.PublishedDate)

            // İlişkiler (FK'lar)
            .Map(dest => dest.WriterId, src => src.WriterId)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.PublisherId, src => src.PublisherId)

            // Navigation'ları EF yönetsin
            .Ignore(dest => dest.Writer)
            .Ignore(dest => dest.Category)
            .Ignore(dest => dest.Publisher);

        // Request → Entity
        config.NewConfig<AddBookCommand, Book>()
            // Asıl alanlar
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Summary, src => src.Summary)
            .Map(dest => dest.ISBN, src => src.ISBN)
            .Map(dest => dest.Language, src => src.Language)
            .Map(dest => dest.PageCount, src => src.PageCount)
            .Map(dest => dest.PublishedDate, src => src.PublishedDate)
            // İlişkiler (FK)
            .Map(dest => dest.WriterId, src => src.WriterId)
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.PublisherId, src => src.PublisherId);
    }
}