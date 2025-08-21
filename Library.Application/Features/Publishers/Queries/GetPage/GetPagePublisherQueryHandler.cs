using System.Linq.Expressions;
using Library.Application.Common.Expressions;
using Library.Application.Dto.Abstractions;
using Library.Application.Dto.PublisherDtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LinqKit;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Publishers.Queries.GetPage;

internal sealed class GetPagePublisherQueryHandler(IPublisherRepository repository)
    : IRequestHandler<GetPagePublisherQuery, Result<BasePageResponseDto<PublisherDto>>>
{
    public async Task<Result<BasePageResponseDto<PublisherDto>>> Handle(GetPagePublisherQuery request, CancellationToken cancellationToken)
    {
        // 1) Filtreler
        var nameFilter    = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Name,    x => x.Name);
        var websiteFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Website, x => x.Website);
        var addressFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Address, x => x.Address);
        var countryFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Country, x => x.Country);

        var finalFilter = nameFilter
            .And(websiteFilter)
            .And(addressFilter)
            .And(countryFilter);

        // 2) Sıralama map'i
        var orderMap = new Dictionary<string, Expression<Func<Publisher, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]    = x => x.Name,
            ["website"] = x => x.Website ?? string.Empty,
            ["address"] = x => x.Address ?? string.Empty,
            ["country"] = x => x.Country ?? string.Empty,
            ["id"]      = x => x.Id
        };

        var orderByExpr = QueryExpressionBuilder.BuildOrderBy(
            request.OrderByField, orderMap, defaultKey: "id");

        // 3) Projection'lı paging çağrısı
        var (items, totalCount) = await repository.GetPagedAsync(
            pageNumber:  request.PageNumber,
            pageSize:    request.PageSize,
            filter:      finalFilter,
            orderBy:     orderByExpr,
            isDescending: !request.OrderByAsc,
            getAllData:  request.GetAllData,
            selector:    p => new PublisherDto(
                p.Name,
                p.Website,
                p.Address,
                p.Country,
                p.Books!.Count
            )
            {
                Id        = p.Id,
                Version   = p.Version,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedBy = p.UpdatedBy
            },
            cancellationToken: cancellationToken
        );

        // 4) Artık Adapt yok; items zaten DTO
        var result = new BasePageResponseDto<PublisherDto>
        {
            List         = items.ToList(),
            TotalCount   = totalCount,
            PageNumber   = request.PageNumber,
            PageSize     = request.PageSize,
            OrderByField = request.OrderByField,
            OrderByAsc   = request.OrderByAsc,
            GetAllData   = request.GetAllData
        };

        return result;
    }
}
