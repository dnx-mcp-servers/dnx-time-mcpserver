using Dnx.TimeMcpServer.Models;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
using System.ComponentModel;

namespace Dnx.TimeMcpServer.Tools;

/// <summary>
/// Provides utility functions related to time and date manipulation.
/// </summary>
public class TimeTools
{
    private static readonly LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.CreateWithInvariantCulture("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

    /// <summary>
    /// Gets the current date and time in a specific timezone. If no timezone is provided, the local timezone is used.
    /// </summary>
    /// <param name="currentTimeInput">An object containing target timezone.</param>
    /// <returns>An object containing the current date and time in ISO format, whether daylight saving time is applied, and the target timezone. Returns an error message if the provided timezone is invalid.</returns>
    [McpServerTool(Name = "get_current_date_time", UseStructuredContent = true), Description("Get current date and time in a specific timezone.")]
    public static TimeResult GetCurrentTime(CurrentTimeInput currentTimeInput)
    {
        var targetTimezone = currentTimeInput?.Timezone ?? DateTimeZoneProviders.Tzdb.GetSystemDefault().Id;

        try
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb[targetTimezone];
            var instant = SystemClock.Instance.GetCurrentInstant();
            var zonedDateTime = instant.InZone(dateTimeZone);

            return new TimeResult
            {
                Timezone = targetTimezone,
                IsoTime = _localDateTimePattern.Format(zonedDateTime.LocalDateTime),
                IsDst = zonedDateTime.IsDaylightSavingTime()
            };
        }
        catch (KeyNotFoundException)
        {
            throw new McpException($"Invalid timezone: {targetTimezone}");
        }
        catch (Exception)
        {
            throw new McpException($"Something went wrong, try again.");
        }
    }

    /// <summary>
    /// Converts a time from one timezone to another.
    /// </summary>
    /// <param name="timeConversionInput">An object containing the time to convert, source timezone, and target timezone.</param>
    /// <returns>The converted time and details about the conversion.</returns>
    [McpServerTool(Name = "convert_date_time", UseStructuredContent = true), Description("Convert date and time between timezones.")]
    public static TimeConversionResult ConvertTime(TimeConversionInput timeConversionInput)
    {
        try
        {
            var timePattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm");
            var localTime = timePattern.Parse(timeConversionInput.Time.Trim());
            if (!localTime.Success)
            {
                throw new McpException($"Invalid time format. Use HH:MM in 24-hour format (e.g., 14:30).");
            }

            DateTimeZone sourceZone;
            try
            {
                sourceZone = !string.IsNullOrWhiteSpace(timeConversionInput.SourceTimezone)
                    ? DateTimeZoneProviders.Tzdb[timeConversionInput.SourceTimezone]
                : DateTimeZoneProviders.Tzdb.GetSystemDefault();
            }
            catch (KeyNotFoundException)
            {
                throw new McpException($"Invalid or unsupported source timezone: {timeConversionInput.SourceTimezone}");
            }

            DateTimeZone targetZone;
            try
            {
                targetZone = !string.IsNullOrWhiteSpace(timeConversionInput.TargetTimezone)
                    ? DateTimeZoneProviders.Tzdb[timeConversionInput.TargetTimezone]
                : DateTimeZoneProviders.Tzdb.GetSystemDefault();
            }
            catch (KeyNotFoundException)
            {
                throw new McpException($"Invalid or unsupported target timezone: {timeConversionInput.TargetTimezone}");
            }

            var sourceToday = SystemClock.Instance.InZone(sourceZone).GetCurrentDate();
            var localDateTime = sourceToday + localTime.Value;

            var zonedSource = localDateTime.InZoneLeniently(sourceZone);

            var zonedTarget = zonedSource.WithZone(targetZone);

            var offsetSource = zonedSource.Offset.ToTimeSpan();
            var offsetTarget = zonedTarget.Offset.ToTimeSpan();
            var difference = offsetTarget - offsetSource;
            var hoursDifference = difference.TotalHours;

            string timeDiffStr = hoursDifference % 1 == 0
                                 ? $"{(int)hoursDifference:+#;-#}h"
                                 : $"{hoursDifference:+0.##;-0.##}h";

            var result = new TimeConversionResult
            {
                Source = new TimeResult
                {
                    Timezone = timeConversionInput.SourceTimezone!,
                    IsoTime = _localDateTimePattern.Format(zonedSource.LocalDateTime),
                    IsDst = zonedSource.IsDaylightSavingTime()
                },
                Target = new TimeResult
                {
                    Timezone = timeConversionInput.TargetTimezone!,
                    IsoTime = _localDateTimePattern.Format(zonedTarget.LocalDateTime),
                    IsDst = zonedTarget.IsDaylightSavingTime()
                },

                TimeDifference = timeDiffStr
            };
            return result;
        }
        catch (Exception)
        {
            throw new McpException($"Something went wrong, try again.");
        }
    }
}