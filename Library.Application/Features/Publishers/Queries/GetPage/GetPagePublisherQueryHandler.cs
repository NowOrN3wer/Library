using System.Linq.Expressions;
using Library.Application.Common.Expressions;
using Library.Application.Dto.Abstractions;
using Library.Application.Dto.PublisherDtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Library.Application.Features.Publishers.Queries.GetPage;

internal sealed class GetPagePublisherQueryHandler(
    IPublisherRepository publisherRepository,
    IBookRepository bookRepository // <-- kitap sayıları için eklendi
) : IRequestHandler<GetPagePublisherQuery, Result<BasePageResponseDto<PublisherDto>>>
{
    public async Task<Result<BasePageResponseDto<PublisherDto>>> Handle(GetPagePublisherQuery request,
        CancellationToken cancellationToken)
    {
        // 1) Filtreler
        var nameFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Name, x => x.Name);
        var websiteFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Website, x => x.Website);
        var addressFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Address, x => x.Address);
        var countryFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Country, x => x.Country);
        var finalFilter = nameFilter.And(websiteFilter).And(addressFilter).And(countryFilter);

        // 2) Sıralama map'i
        var orderMap = new Dictionary<string, Expression<Func<Publisher, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = x => x.Name,
            ["website"] = x => x.Website ?? string.Empty,
            ["address"] = x => x.Address ?? string.Empty,
            ["country"] = x => x.Country ?? string.Empty,
            ["id"] = x => x.Id
        };
        var orderByExpr = QueryExpressionBuilder.BuildOrderBy(request.OrderByField, orderMap, "id");

        // 3) SAYIMSIZ projection (hafif)
        var (pageItemsLight, totalCount) = await publisherRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            finalFilter,
            orderByExpr,
            !request.OrderByAsc,
            request.GetAllData,
            p => new
            {
                p.Id, p.Name, p.Website, p.Address, p.Country,
                p.Version, p.CreatedAt, p.UpdatedAt, p.CreatedBy, p.UpdatedBy
            },
            cancellationToken
        );

        var pageList = pageItemsLight.ToList();
        var ids = pageList.Select(x => x.Id).ToList();

        // 4) TEK sorguda sayılar (sadece bu sayfadaki yayınevleri için)
        var countDict = ids.Count == 0
            ? new Dictionary<Guid, int>()
            : await bookRepository.GetAll() // IQueryable<Book>
                .Where(b => ids.Contains(b.PublisherId))
                .GroupBy(b => b.PublisherId)
                .Select(g => new { PublisherId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PublisherId, x => x.Count, cancellationToken);

        // 5) DTO oluştur
        var dtoList = pageList.Select(p => new PublisherDto(
                p.Name, p.Website, p.Address, p.Country,
                countDict.TryGetValue(p.Id, out var c) ? c : 0
            )
            {
                Id = p.Id,
                Version = p.Version,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedBy = p.UpdatedBy
            })
            .ToList();

        return new BasePageResponseDto<PublisherDto>
        {
            List = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            OrderByField = request.OrderByField,
            OrderByAsc = request.OrderByAsc,
            GetAllData = request.GetAllData
        };
    }
}