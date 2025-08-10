namespace Library.Application.Dto.CommonDtos;

public sealed record LookupItemDto<TKey>(TKey Id, string Text);