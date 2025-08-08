using Library.Application.Dto.WriterDtos;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetPage;

internal sealed class GetPageWriterQueryHandler(
    IWriterRepository repository) : IRequestHandler<GetPageWriterQuery, Result<BasePageResponseDto<WriterDto>>>
{
    public async Task<Result<BasePageResponseDto<WriterDto>>> Handle(GetPageWriterQuery request,
        CancellationToken cancellationToken)
    {
        var filterExpression = GetPageWriterQueryExpressionBuilder.BuildFilter(request);
        var orderByExpression = GetPageWriterQueryExpressionBuilder.BuildOrderBy(request.OrderByField);

        var (items, totalCount) = await repository.GetPagedAsync(
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            filter: filterExpression,
            orderBy: orderByExpression,
            isDescending: !request.OrderByAsc,
            getAllData: request.GetAllData
        );

        var resultItems = items.Adapt<List<WriterDto>>();

        var result = new BasePageResponseDto<WriterDto>
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