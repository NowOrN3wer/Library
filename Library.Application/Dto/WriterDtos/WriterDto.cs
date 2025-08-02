using Library.Application.Dto.Abstractions;

namespace Library.Application.Dto.WriterDtos;

public sealed record WriterDto(
    string firstName,
    string? lastName,
    string? biography,
    string? nationality,
    DateTimeOffset? birthDate,
    DateTimeOffset? deathDate,
    string? website,
    string? email
) : EntityDto;

