namespace TUI_Reader;

/// <summary>
/// Contains the login credentials for jil.travel.
/// </summary>
public class LoginContext
{
	/// <summary>
	/// Email to be used when logging in.
	/// </summary>
	public string Email { get; init; }
	/// <summary>
	/// Password to be used when logging in.
	/// </summary>
	public string Password { get; init; }
}