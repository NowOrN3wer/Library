public record class BasePageResponseDto<T>
{
    public List<T>? List { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
    public string? OrderByField { get; init; }
    public bool OrderByAsc { get; init; }
    public bool GetAllData { get; init; }
}