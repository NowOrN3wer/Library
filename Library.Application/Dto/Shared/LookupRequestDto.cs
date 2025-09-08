using System.ComponentModel.DataAnnotations;

namespace Library.Application.Dto.Shared;

public abstract record LookupRequestDto<TId>
{
    public string? Q { get; set; }

    [Range(1, 100)] public int Limit { get; set; } = 20;

    public List<TId>? IncludeIds { get; set; }

    /// <summary>
    ///     Base64-encoded cursor produced by the previous response (NextCursor).
    /// </summary>
    public string? Cursor { get; set; }
}