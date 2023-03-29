using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using DriverOptions = TUI_Reader.Contracts.DriverOptions;

namespace TUI_Reader.Drivers;

/// <summary>
/// A webdriver with a unique id. Is used for different operations.
/// </summary>
internal class Driver : IDisposable
{
	/// <summary>
	/// Unique ID.
	/// </summary>
	public Guid Id { get; } = Guid.NewGuid();
	/// <summary>
	/// Web webDriver.
	/// </summary>
	public IWebDriver WebDriver { get; }
	private DriverOptions DriverOptions { get; }
	private Driver(IWebDriver webDriver, DriverOptions driverOptions)
	{
		DriverOptions = driverOptions;
		WebDriver = webDriver;
	}
	/// <summary>
	/// Creates a webDriver.
	/// </summary>
	/// <returns><see cref="Driver"/> that uses chrome as webdriver.</returns>
	public static Driver Chrome(DriverOptions driverOptions)
	{
		var chromeOptions = new ChromeOptions();
		if(!driverOptions.Logging) DisableLogging(chromeOptions);
		if(driverOptions.Headless) chromeOptions.AddArgument("--headless");
		return new Driver(new ChromeDriver(chromeOptions), driverOptions);
	}
	/// <summary>
	/// Disables logging.
	/// </summary>
	/// <param name="options"><see cref="ChromeOptions"/></param>
	private static void DisableLogging(ChromeOptions options)
	{
		options.SetLoggingPreference(LogType.Browser, LogLevel.Off);
		options.SetLoggingPreference(LogType.Driver, LogLevel.Off);
		options.SetLoggingPreference(LogType.Performance, LogLevel.Off);
		options.AddArgument("log-level=3");
	}
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public void Dispose() => WebDriver.Quit();
}