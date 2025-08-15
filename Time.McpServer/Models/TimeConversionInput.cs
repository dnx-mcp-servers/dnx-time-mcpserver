using System.ComponentModel;

namespace Dnx.TimeMcpServer.Models;

/// <summary>
/// Represents the input parameters for a time conversion operation.
/// </summary>
public record TimeConversionInput
{
    /// <summary>
    /// Gets or initializes the time string to be converted.
    /// </summary>
    [Description("Time to convert in 24-hour format (HH:MM).")]
    public required string Time { get; init; }

    /// <summary>
    /// Gets or initializes the source timezone of the input time, if specified.
    /// </summary>
    [Description("Source IANA timezone name (e.g., 'America/New_York', 'Europe/London'). If not provided, the system's default timezone is used.")]
    public string? SourceTimezone { get; init; }

    /// <summary>
    /// Gets or initializes the target timezone to which the time should be converted, if specified.
    /// </summary>
    [Description("Target IANA timezone name (e.g., 'Asia/Tokyo', 'America/San_Francisco'). If not provided, the system's default timezone is used.")]
    public string? TargetTimezone { get; init; }
};