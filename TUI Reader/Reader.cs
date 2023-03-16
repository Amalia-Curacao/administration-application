using System.Text.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TUI_Reader.Actions;
using TUI_Reader.Contracts;
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
        var options = new ChromeOptions();
        if(headless) options.AddArgument("--headless");
        options.SetLoggingPreference(LogType.Browser, LogLevel.Off);
        options.SetLoggingPreference(LogType.Driver, LogLevel.Off);
        return new ChromeDriver(options);
    }
}