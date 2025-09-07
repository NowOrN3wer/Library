using System.Linq.Expressions;
using Library.Application.Common.Expressions;
using Library.Application.Dto.Abstractions;
using Library.Application.Dto.CategoryDtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LinqKit;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Queries.GetPage;

internal sealed class GetPageCategoryQueryHandler(ICategoryRepository repository)
    : IRequestHandler<GetPageCategoryQuery, Result<BasePageResponseDto<CategoryDto>>>
{
    public async Task<Result<BasePageResponseDto<CategoryDto>>> Handle(GetPageCategoryQuery request,
        CancellationToken cancellationToken)
    {
        // 1) Filtreler: Name ve Description ayrı ayrı (boşsa no-op döner)
        var nameFilter = QueryExpressionBuilder.BuildContainsFilter<Category>(
            request.Name,
            x => x.Name
        );

        var descFilter = QueryExpressionBuilder.BuildContainsFilter<Category>(
            request.Description,
            x => x.Description
        );

        // Birleştir (ikisi de true ise etkisiz)
        var finalFilter = nameFilter.And(descFilter);

        // 2) Order map
        var orderMap = new Dictionary<string, Expression<Func<Category, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = x => x.Name,
            ["description"] = x => x.Description ?? string.Empty,
            ["id"] = x => x.Id
        };

        var orderByExpr = QueryExpressionBuilder.BuildOrderBy(
            request.OrderByField, orderMap, "id");

        // 3) Repository çağrısı
        var (items, totalCount) = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            finalFilter,
            orderByExpr,
            !request.OrderByAsc,
            request.GetAllData
        );

        // 4) Map + response
        var resultItems = items.Adapt<List<CategoryDto>>();

        var result = new BasePageResponseDto<CategoryDto>
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