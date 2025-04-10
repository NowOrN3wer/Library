using System.Runtime.InteropServices;

namespace Library.Application.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTimeOffset ToTurkeyTime(this DateTimeOffset value)
    {
        var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Turkey Standard Time"
            : "Europe/Istanbul";

        var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return TimeZoneInfo.ConvertTime(value, turkeyTimeZone);
    }
}