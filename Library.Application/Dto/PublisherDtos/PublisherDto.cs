using Library.Application.Dto.Abstractions;

namespace Library.Application.Dto.PublisherDtos;

public sealed record PublisherDto(
    string Name,
    string? Website,
    string? Address,
    string? Country,
    long BookCount
) : EntityDto;