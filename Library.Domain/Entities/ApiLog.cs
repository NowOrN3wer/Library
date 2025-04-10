namespace CArch.Domain.Entities;

public sealed class ApiLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? IPAddress { get; set; }
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public DateTime RequestTime { get; set; }
    public DateTime ResponseTime { get; set; }
}
