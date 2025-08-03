using Library.Application.Dto.Abstractions;
using Library.Application.Dto.WriterDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.GetPage;

public sealed record GetPageWriterCommand : BasePageRequestDto, IRequest<Result<BasePageResponseDto<WriterDto>>>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Nationality { get; init; }
    public string? Biography { get; init; }
    public string? Website { get; init; }
    public DateTimeOffset? BirthDateFrom { get; init; }
    public DateTimeOffset? BirthDateTo { get; init; }
    public bool? IsDead { get; init; }
}