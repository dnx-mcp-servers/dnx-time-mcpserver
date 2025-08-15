using System.ComponentModel;

namespace Dnx.TimeMcpServer.Models;

/// <summary>
/// Represents a request to get the current time in a specific timezone.
///     
public record CurrentTimeInput
{
    /// <summary>
    /// Gets or initializes the timezone for which the current time is requested.
    /// </summary>
    [Description("IANA timezone name (e.g., 'America/New_York', 'Europe/London'). If not provided, the system's default timezone is used.")]
    public string? Timezone { get; init; } = Environment.GetEnvironmentVariable("local-timezone") ?? default!;
}