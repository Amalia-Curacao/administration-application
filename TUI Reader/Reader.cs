using System.Text.Json;
using TUI_Reader.Actions;
using TUI_Reader.Contracts;
using TUI_Reader.Database;
using TUI_Reader.Properties;

namespace TUI_Reader;

public sealed class Reader
{
    /// <summary>
    /// ./Properties/appsettings.json parsed as an Object.
    /// </summary>
    private readonly static AppSettings AppSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./Properties/appsettings.json"))!;
    /// <summary>
    /// Database instance where notifications will be saved.
    /// </summary>
    private Context DatabaseContext { get; init; } = new();
	/// <summary>
	/// <inheritdoc cref="ReaderOptions"/>
	/// </summary>
	public ReaderOptions ReaderOptions { get; init; } = new();
	/// <summary>
	/// <inheritdoc cref="LoginContext"/>
	/// </summary>
	private LoginContext LoginContext { get; }
	/// <summary>
	/// <inheritdoc cref="Login"/>
	/// </summary>
	private readonly Login Login;
	/// <summary>
	/// <inheritdoc cref="ReadOpenedNotifications"/>
	/// </summary>
	private readonly ReadOpenedNotifications ReadOpenedNotifications;
	/// <summary>
	/// Instantiates <see cref="Reader"/> 
	/// </summary>
	public Reader(LoginContext? loginContext = null)
	{
		LoginContext = loginContext ?? new LoginContext
		{
			Email = AppSettings.Email,
			Password = AppSettings.Password
		};
		Login = new Login
		{
			LoginContext = LoginContext
		};
		ReadOpenedNotifications = new ReadOpenedNotifications
		{
			ReaderOptions = ReaderOptions,
			Login = Login
		};
	}
    /// <summary>
    /// Runs all the operations that needed to be able to read the notifications.
    /// </summary>
	private async Task<IEnumerable<Notification>> Run()
		=> new List<Notification>(
			await ReadOpenedNotifications.Run()
			);

	/// <summary>
    /// Gets all notifications currently in the database.
    /// </summary>
    /// <returns>All saved notifications.</returns>
    public async Task<IEnumerable<Notification>> GetNotifications() => await Run();
}