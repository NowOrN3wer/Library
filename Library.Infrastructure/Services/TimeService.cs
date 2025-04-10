using System.Runtime.InteropServices;
using Library.Application.Common.Interfaces;

namespace Library.Infrastructure.Services;

public class TimeService : ITimeService
{
    private static readonly TimeZoneInfo TurkeyTimeZone = GetTurkeyTimeZone();

    public DateTimeOffset Now => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TurkeyTimeZone);
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    private static TimeZoneInfo GetTurkeyTimeZone()
    {
        var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Turkey Standard Time"       // Windows
            : "Europe/Istanbul";           // Linux/macOS

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new InvalidOperationException($"'{timeZoneId}' saat dilimi bulunamadı. Lütfen sistem saat dilimini kontrol edin.");
        }
    }
}