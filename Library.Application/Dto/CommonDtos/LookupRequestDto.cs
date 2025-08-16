using System.ComponentModel.DataAnnotations;

namespace Library.Application.Dto.CommonDtos;

public abstract record LookupRequestDto<TId>
{
    public string? Q { get; set; }

    [Range(1, 100)]
    public int Limit { get; set; } = 20;

    public List<TId>? IncludeIds { get; set; }
}