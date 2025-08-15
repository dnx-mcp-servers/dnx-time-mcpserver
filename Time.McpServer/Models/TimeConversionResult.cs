using System.ComponentModel;

namespace Dnx.TimeMcpServer.Models;

/// <summary>
/// Represents the result of a time conversion operation.
/// </summary>
public record TimeConversionResult
{
    /// <summary>
    /// Gets or initializes the source time result.
    /// </summary>
    [Description("The original time result before conversion.")]
    public required TimeResult Source { get; init; }

    /// <summary>
    /// Gets or initializes the target time result after conversion.
    /// </summary>
    [Description("The converted time result.")]
    public required TimeResult Target { get; init; }

    /// <summary>
    /// Gets or initializes the string representation of the time difference between source and target.
    /// </summary>
    [Description("A human-readable string indicating the difference in time between the source and target results.")]
    public required string TimeDifference { get; init; }
}