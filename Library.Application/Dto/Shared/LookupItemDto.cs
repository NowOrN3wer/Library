namespace Library.Application.Dto.Shared;

public sealed record LookupItemDto<TKey>(TKey Id, string Text);