namespace TUI_Reader.Properties;

internal sealed class AppSettings
{
    /// <summary>
    /// Email used to log in to TUI.
    /// </summary>
    public string Email { get; init; } = null!;
    /// <summary>
    /// Password used to log in to TUI.
    /// </summary>
    public string Password { get; init; } = null!;
}