# Time MCP Server

A Model Context Protocol server that provides time and timezone conversion capabilities. This server enables LLMs to get current time information and perform timezone conversions using IANA timezone names, with automatic system timezone detection.

### Available Tools

- `get_current_date_time` - Get current date and time in a specific timezone or system timezone.
  - Required arguments:
    - `timezone` (string): IANA timezone name (e.g., 'America/New_York', 'Europe/London')

- `convert_time` - Convert time between timezones.
  - Required arguments:
    - `source_timezone` (string): Source IANA timezone name
    - `time` (string): Time in 24-hour format (HH:MM)
    - `target_timezone` (string): Target IANA timezone name

## Examples of Questions

1. "What time is it now?" (will use system timezone)
2. "What time is it in Tokyo?"
3. "When it's 4 PM in New York, what time is it in London?"
4. "Convert 9:30 AM Tokyo time to New York time"