namespace Library.Application.Dto.Shared;

public sealed record LookupResponseDto<TId>(
    IReadOnlyList<LookupItemDto<TId>> Items,
    string? NextCursor,
    bool HasMore
);
