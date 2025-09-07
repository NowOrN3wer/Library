using System.Linq.Expressions;
using Library.Application.Common.Expressions;
using Library.Application.Dto.Abstractions;
using Library.Application.Dto.BookDtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LinqKit;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Queries.GetPage;

internal sealed class GetPageBookQueryHandler(IBookRepository repository)
    : IRequestHandler<GetPageBookQuery, Result<BasePageResponseDto<BookDto>>>
{
    public async Task<Result<BasePageResponseDto<BookDto>>> Handle(
        GetPageBookQuery request,
        CancellationToken cancellationToken)
    {
        // 1) Filtreler (stringlerde Contains, sayısal/tarihte eşitlik)
        var writerFirstNameFilter =
            QueryExpressionBuilder.BuildContainsFilter<Book>(request.WriterName, b => b.Writer.FirstName);
        var writerLastNameFilter =
            QueryExpressionBuilder.BuildContainsFilter<Book>(request.WriterName, b => b.Writer.LastName);
        var categoryFilter =
            QueryExpressionBuilder.BuildContainsFilter<Book>(request.CategoryName, b => b.Category.Name);
        var publisherFilter =
            QueryExpressionBuilder.BuildContainsFilter<Book>(request.PublisherName, b => b.Publisher.Name);
        var titleFilter = QueryExpressionBuilder.BuildContainsFilter<Book>(request.Title, b => b.Title);
        var summaryFilter = QueryExpressionBuilder.BuildContainsFilter<Book>(request.Summary, b => b.Summary);
        var isbnFilter = QueryExpressionBuilder.BuildContainsFilter<Book>(request.ISBN, b => b.ISBN);
        var langFilter = QueryExpressionBuilder.BuildContainsFilter<Book>(request.Language, b => b.Language);

        // Eşitlik bazlı (varsa uygula)
        Expression<Func<Book, bool>> pageCountFilter = b => true;
        if (request.PageCount.HasValue)
            pageCountFilter = b => b.PageCount == request.PageCount.Value;

        Expression<Func<Book, bool>> publishedDateFilter = b => true;
        if (request.PublishedDate.HasValue)
            publishedDateFilter = b => b.PublishedDate == request.PublishedDate.Value;

        var finalFilter = writerFirstNameFilter
            .And(writerLastNameFilter)
            .And(categoryFilter)
            .And(publisherFilter)
            .And(titleFilter)
            .And(summaryFilter)
            .And(isbnFilter)
            .And(langFilter)
            .And(pageCountFilter)
            .And(publishedDateFilter);

        // 2) Sıralama map'i
        var orderMap = new Dictionary<string, Expression<Func<Book, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["writerName"] = b => b.Writer.FullName,
            ["categoryName"] = b => b.Category.Name,
            ["publisherName"] = b => b.Publisher.Name,
            ["title"] = b => b.Title,
            ["isbn"] = b => b.ISBN ?? string.Empty,
            ["language"] = b => b.Language ?? string.Empty,
            ["pageCount"] = b => b.PageCount ?? 0,
            ["publishedDate"] = b => b.PublishedDate ?? DateTimeOffset.MinValue,
            ["id"] = b => b.Id
        };

        var orderByExpr = QueryExpressionBuilder.BuildOrderBy(
            request.OrderByField, orderMap, "id");

        // 3) Projection'lı paging çağrısı (DTO doğrudan seçiliyor)
        var (items, totalCount) = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            finalFilter,
            orderByExpr,
            !request.OrderByAsc,
            request.GetAllData,
            b => new BookDto(
                b.WriterId,
                b.Writer.FullName,
                b.CategoryId,
                b.Category.Name,
                b.PublisherId,
                b.Publisher.Name,
                b.Title,
                b.Summary,
                b.ISBN,
                b.Language,
                b.PageCount,
                b.PublishedDate
            )
            {
                Id = b.Id,
                Version = b.Version,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                CreatedBy = b.CreatedBy,
                UpdatedBy = b.UpdatedBy
            },
            cancellationToken
        );

        // 4) Sayfalı cevap
        var result = new BasePageResponseDto<BookDto>
        {
            List = items.ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            OrderByField = request.OrderByField,
            OrderByAsc = request.OrderByAsc,
            GetAllData = request.GetAllData
        };

        return result;
    }
}