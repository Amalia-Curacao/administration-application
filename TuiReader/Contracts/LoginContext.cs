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

	public static LoginContext DefaultLoginContext()
	{
		var appsettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./Properties/appsettings.json"))!;
		return new LoginContext
		{
			Email = appsettings.Email,
			Password = appsettings.Password
		};
	}
}