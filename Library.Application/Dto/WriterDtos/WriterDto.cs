using Library.Application.Dto.Abstractions;

namespace Library.Application.Dto.WriterDtos;

public sealed record WriterDto(
    string FirstName,
    string? LastName,
    string? Biography,
    string? Nationality,
    DateTimeOffset? BirthDate,
    DateTimeOffset? DeathDate,
    string? Website,
    string? Email
) : EntityDto;