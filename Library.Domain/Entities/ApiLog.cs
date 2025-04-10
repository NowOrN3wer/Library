namespace Library.Domain.Entities;

public sealed class ApiLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? IPAddress { get; set; }
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public DateTimeOffset RequestTime { get; set; }
    public DateTimeOffset ResponseTime { get; set; }
}
