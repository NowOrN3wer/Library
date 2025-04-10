namespace Library.Application.Dto.ApiLogDTOs;

public class ApiLogDto
{
    public Guid id { get; set; } = Guid.NewGuid();
    public string? iPAddress { get; set; }
    public string? path { get; set; }
    public string? method { get; set; }
    public object? requestBody { get; set; }
    public object? responseBody { get; set; }
    public int statusCode { get; set; }
    public DateTimeOffset requestTime { get; set; }
    public DateTimeOffset responseTime { get; set; }
}