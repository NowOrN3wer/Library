using System.Linq.Expressions;
using AutoMapper;
using Library.Application.Dto.ApiLogDTOs;
using LinqKit;
using Library.Application.DTO.ApiLogDTOs;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.ApiLogs.GetPage;

public sealed class GetPageApiLogCommandHandler(IApiLogRepository repository, IMapper mapper)
    : IRequestHandler<GetPageApiLogCommand, Result<ApiLogPageResponseDto>>
{
    public async Task<Result<ApiLogPageResponseDto>> Handle(GetPageApiLogCommand request,
        CancellationToken cancellationToken)
    {
        Expression<Func<ApiLog, bool>> filter = x => true;

        if (!string.IsNullOrWhiteSpace(request.iPAddress))
        {
            filter = filter.And(x => x.IPAddress == request.iPAddress);
        }

        if (!string.IsNullOrWhiteSpace(request.path))
        {
            filter = filter.And(x => x.Path == request.path);
        }

        if (!string.IsNullOrWhiteSpace(request.method))
        {
            filter = filter.And(x => x.Method == request.method);
        }

        if (request.statusCode.HasValue)
        {
            filter = filter.And(x => x.StatusCode == request.statusCode.Value);
        }

        if (request.startTime.HasValue && request.endTime.HasValue)
        {
            filter = filter.And(x =>
                x.RequestTime >= request.startTime.Value && x.RequestTime <= request.endTime.Value);
        }

        var orderBy = GetOrderByExpression(request.orderByField);

        var pageResult = await repository.GetPagedAsync(request.pageNumber, request.pageSize, filter, orderBy,
            request.isDescending);
        var result = new ApiLogPageResponseDto
        {
            list = mapper.Map<List<ApiLogDto>>(pageResult.items),
            totalCount = pageResult.totalCount,
            pageNumber = request.pageNumber,
            pageSize = request.pageSize,
            OrderByField = request.orderByField,
            OrderByAsc = !request.isDescending
        };

        return result;
    }

    private static Expression<Func<ApiLog, object>>? GetOrderByExpression(string? fieldName)
    {
        return fieldName?.ToLower() switch
        {
            "ipaddress" => x => x.IPAddress ?? string.Empty,
            "path" => x => x.Path ?? string.Empty,
            "method" => x => x.Method ?? string.Empty,
            "statuscode" => x => x.StatusCode,
            "requestime" => x => x.RequestTime,
            "requesttime" => x => x.ResponseTime,
            _ => null
        };
    }
}