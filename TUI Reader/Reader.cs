using System.Text.Json;
using TUI_Reader.Actions;
using TUI_Reader.Contracts;
using TUI_Reader.Database;
using TUI_Reader.Properties;

namespace TUI_Reader;

public sealed class Reader
{
    private readonly Login             _login;
    private readonly ReadOpenedNotifications _readOpenedNotifications;
    public Reader(bool headless = true)
    {
        var appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./Properties/appsettings.json"));
        var driver = CreateWebDriver(headless);
        _login = new Login(appSettings!.Email, appSettings.Password, driver);
        _readOpenedNotifications = new ReadOpenedNotifications(driver);
    }
    public Reader(string email, string password, bool headless = true)
    {
        var driver = CreateWebDriver(headless);
        _login = new Login(email, password, driver);
        _readOpenedNotifications = new ReadOpenedNotifications(driver);
    }
    public IEnumerable<Notification> Run()
    {
        _login.Run(5);
        return _readOpenedNotifications.Run();
    }
    private static IWebDriver CreateWebDriver(bool headless)
    {
		return new List<Notification>(ReadOpenedNotifications.Run(ReaderOptions));
    }

    /// <summary>
    /// Gets all notifications currently in the database.
    /// </summary>
    /// <returns>All saved notifications.</returns>
    public async Task<IEnumerable<Notification>> GetNotifications() => await Run();
}