namespace Library.Application.Common;

public interface ITimeService
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }
}