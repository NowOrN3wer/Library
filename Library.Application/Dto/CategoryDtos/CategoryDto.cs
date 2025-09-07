using Library.Application.Dto.Abstractions;

namespace Library.Application.Dto.CategoryDtos;

public sealed record CategoryDto(
    string Name,
    string? Description
) : EntityDto;