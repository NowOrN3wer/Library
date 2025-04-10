namespace Library.Application.Common.Interfaces;

public interface ITimeService
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }
}