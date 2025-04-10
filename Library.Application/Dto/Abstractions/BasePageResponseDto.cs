namespace Library.Application.Dto.Abstractions;

public abstract class BasePageResponseDto<T>
{
    public List<T>? list { get; set; } = new();
    public int totalCount { get; set; }
    public int pageNumber { get; set; }
    public int pageSize { get; set; }
    public int totalPages => (int)Math.Ceiling((double)totalCount / pageSize);
    public bool hasNextPage => pageNumber < totalPages;
    public bool hasPreviousPage => pageNumber > 1;
    public string? OrderByField { get; set; }
    public bool OrderByAsc { get; set; }
}