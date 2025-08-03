namespace Library.Application.Dto.Abstractions;

public record class BasePageRequestDto
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? OrderByField { get; set; }

    public bool OrderByAsc { get; set; } = true;

    public bool GetAllData { get; set; } = false;
}