using Library.Application.Dto.Abstractions;
using Library.Application.Dto.CategoryDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Categories.Queries.GetPage;

public sealed record GetPageCategoryQuery(string? Name, string? Description)
    : BasePageRequestDto, IRequest<Result<BasePageResponseDto<CategoryDto>>>;