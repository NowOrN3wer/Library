using System.Linq.Expressions;
using Library.Application.Common.Expressions;
using Library.Application.Dto.Abstractions;
using Library.Application.Dto.PublisherDtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LinqKit;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Publishers.Queries.GetPage;

internal sealed class GetPagePublisherQueryHandler(IPublisherRepository repository)
    : IRequestHandler<GetPagePublisherQuery, Result<BasePageResponseDto<PublisherDto>>>
{
    public async Task<Result<BasePageResponseDto<PublisherDto>>> Handle(GetPagePublisherQuery request, CancellationToken cancellationToken)
    {
        var nameFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Name,     x => x.Name);
        var websiteFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Website, x => x.Website);
        var addressFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Address, x => x.Address);
        var countryFilter = QueryExpressionBuilder.BuildContainsFilter<Publisher>(request.Country, x => x.Country);

        // Tümünü AND'le
        var finalFilter = nameFilter
            .And(websiteFilter)
            .And(addressFilter)
            .And(countryFilter);

        // 2) Order map
        var orderMap = new Dictionary<string, Expression<Func<Publisher, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]        = x => x.Name,
            ["website"] = x => x.Website ?? string.Empty,
            ["address"] = x => x.Address ?? string.Empty,
            ["country"] = x => x.Country ?? string.Empty,
            ["id"]          = x => x.Id
        };

        var orderByExpr = QueryExpressionBuilder.BuildOrderBy(
            request.OrderByField, orderMap, defaultKey: "id");

        // 3) Repository çağrısı
        var (items, totalCount) = await repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            filter: finalFilter,   
            orderBy: orderByExpr, 
            isDescending: !request.OrderByAsc,
            getAllData: request.GetAllData
        );

        // 4) Map + response
        var resultItems = items.Adapt<List<PublisherDto>>();

        var result = new BasePageResponseDto<PublisherDto>
        {
            List = resultItems,
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