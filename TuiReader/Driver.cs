using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TuiReader;

/// <summary>
/// A webdriver with a unique id. Is used for different operations.
/// </summary>
internal class Driver : IDisposable
{
	/// <summary>
	/// Last id used for an operation.
	/// </summary>
	public static int LastId;
	/// <summary>
	/// Determines if activities should be logged.
	/// </summary>
	public bool Logging { get; init; }
	/// <summary>
	/// Indicates if driver is logged in to a website.
	/// </summary>
	public bool LoggedIn { get; set; } = false;
	/// <summary>
	/// Unique id. 
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// <inheritdoc cref="IWebDriver"/>
	/// </summary>
	public IWebDriver WebDriver { get; }
	/// <summary>
	/// <inheritdoc cref="DriverOptions"/>
	/// </summary>
	private Contracts.DriverOptions DriverOptions { get; }
	/// <summary>
	/// Instantiates <see cref="Driver"/>.
	/// </summary>
	private Driver(IWebDriver webDriver, Contracts.DriverOptions driverOptions)
	{
		DriverOptions = driverOptions;
		WebDriver = webDriver;
	}
	/// <summary>
	/// Creates a webDriver.
	/// </summary>
	/// <returns><see cref="Driver"/> that uses chrome as webdriver.</returns>
	public static Driver Chrome(Contracts.DriverOptions driverOptions)
	{
		var chromeOptions = new ChromeOptions();
		var chromeServices = ChromeDriverService.CreateDefaultService();
		if(!driverOptions.Logging)
		{
			DisableLoggingOptions(chromeOptions);
			DisableLoggingServices(chromeServices);
		}
		if(driverOptions.Headless) chromeOptions.AddArgument("--headless");
		return new Driver(new ChromeDriver(chromeOptions), driverOptions)
		{
			Logging = driverOptions.Logging,
			Id = LastId++
		};
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