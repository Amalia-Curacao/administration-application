using System.Text.Json;
using TuiReader.Properties;

namespace TuiReader;

/// <summary>
/// Contains the login credentials for jil.travel.
/// </summary>
public class LoginContext
{
	/// <summary>
	/// Email to be used when logging in.
	/// </summary>
	public string Email { get; init; } = null!;
	/// <summary>
	/// Password to be used when logging in.
	/// </summary>
	public string Password { get; init; } = null!;

	/// <summary>
	/// Gets the login context from the appsettings.development.json file.
	/// </summary>
	public static LoginContext DefaultLoginContext()
	{
		var appsettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText($".\\Properties\\appsettings.development.json"))!;
		return new LoginContext
		{
			Email = appsettings.Email,
			Password = appsettings.Password
		};
	}
}