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
	/// <inheritdoc cref="Guid"/>
	/// </summary>
	public Guid Id { get; } = Guid.NewGuid();
	/// <summary>
	/// <inheritdoc cref="IWebDriver"/>
	/// </summary>
	public IWebDriver WebDriver { get; }
	/// <summary>
	/// <inheritdoc cref="DriverOptions"/>
	/// </summary>
	private DriverOptions DriverOptions { get; }
	/// <summary>
	/// Instantiates <see cref="Driver"/>.
	/// </summary>
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
		var chromeServices = ChromeDriverService.CreateDefaultService();
		if(!driverOptions.Logging)
		{
			DisableLoggingOptions(chromeOptions);
			DisableLoggingServices(chromeServices);
		}
		if(driverOptions.Headless) chromeOptions.AddArgument("--headless");
		return new Driver(new ChromeDriver(chromeOptions), driverOptions);
	}
	/// <summary>
	/// Disables logging for <see cref="ChromeOptions"/>.
	/// </summary>
	/// <param name="options"><see cref="ChromeOptions"/></param>
	private static void DisableLoggingOptions(ChromeOptions options)
	{
		options.SetLoggingPreference(LogType.Browser, LogLevel.Off);
		options.SetLoggingPreference(LogType.Driver, LogLevel.Off);
		options.SetLoggingPreference(LogType.Performance, LogLevel.Off);
		options.AddArgument("log-level=3");
	}
	/// <summary>
	/// Disables logging for <see cref="ChromeDriverService"/>
	/// </summary>
	/// <param name="service">
	///	<see cref="ChromeDriverService"/>
	/// </param>
	private static void DisableLoggingServices(ChromeDriverService service)
	{
		service.HideCommandPromptWindow = true;
	}
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public void Dispose() => WebDriver.Quit();
}