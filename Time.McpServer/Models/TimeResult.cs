using System.ComponentModel;

namespace Dnx.TimeMcpServer.Models;

/// <summary>
/// Represents the result of a time-related operation.
/// </summary>
public record TimeResult
{
    /// <summary>
    /// Gets or initializes the timezone information.
    /// </summary>
    [Description("The timezone in which the time is recorded.")]
    public required string Timezone { get; init; }

    /// <summary>
    /// Gets or initializes the ISO 8601 formatted time.
    /// </summary>
    [Description("The ISO 8601 formatted time string.")]
    public required string IsoTime { get; init; }

    /// <summary>
    /// Gets or initializes a value indicating whether daylight saving time is in effect.
    /// </summary>
    [Description("A boolean indicating whether daylight saving time (DST) is currently in effect.")]
    public required bool IsDst { get; init; }
}