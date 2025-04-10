using Library.Application.DTO.ApiLogDTOs;
using MediatR;
using TS.Result;

namespace Library.Application.Features.ApiLogs.GetPage;

public sealed record GetPageApiLogCommand(
    string? iPAddress,
    string? path,
    string? method,
    int? statusCode,
    DateTimeOffset? startTime,
    DateTimeOffset? endTime,
    int pageNumber,
    int pageSize = 10,
    bool isDescending = false,
    string? orderByField = null) : IRequest<Result<ApiLogPageResponseDto>>;