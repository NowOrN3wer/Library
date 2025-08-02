using System.Runtime.InteropServices;

namespace Library.Domain.Helpers;

public static class TimeHelper
{
    public static DateTimeOffset GetTurkeyTime()
    {
        var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "Turkey Standard Time"
                : "Europe/Istanbul");

        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, turkeyTimeZone);
    }
}