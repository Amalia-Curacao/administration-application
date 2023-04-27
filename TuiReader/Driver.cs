using System.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TuiReader.Contracts;
using TuiReader.WebElements;

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
	
	/// <summary>
    /// Gets used for logging to keep track of notifications read.
    /// </summary>
    private static int NotificationNumber;
    /// <summary>
    /// Login to JILL's website.
    /// </summary>
    public Driver Login(LoginContext context)
    {
        if(Logging) Console.WriteLine($"{Id}: login start");
        
        LoggedIn = AttemptLogin(context);
        
        if (!LoggedIn) throw new Exception($"{Id}: login failed");
        
        if (Logging) Console.WriteLine($"{Id}: login successful");
        return this;
    }
    /// <summary>
    /// Attempt login to JILL's website.
    /// </summary>
    /// <returns>
    /// Is true if login was successful.
    /// </returns>
    private bool AttemptLogin(LoginContext context)
    {
		WebDriver.GoToLoginPage();
        // Will attempt to login 20 times before giving up. This is added because TUI has a shit login system.
        for (var i = 0; i < 20; i++)
        {
            WebDriver.ClearCredentials();
            WebDriver.FillCredentials(context);
            WebDriver.GetLoginButton().Click();
            try
            {
                return WebDriver.Wait().Until(wd => wd.IsOnHomePage());
            }
            catch (Exception) { /*ignored*/ }
        }
        throw new Exception("Email address and/or password is incorrect.");
    }
    /// <summary>
    /// Gets the opened notification.
    /// </summary>
    /// <remarks>
    /// Make sure that the <see cref="driver"/> is logged in to the website.
    /// </remarks>
    /// <returns>Opened notification.</returns>
    public Task<Notification> GetNotification(string notificationLink)
    {
        if (string.IsNullOrEmpty(notificationLink)) throw new NoNullAllowedException($"{nameof(notificationLink)} parameter cannot be null or empty.");
		WebDriver.Navigate().GoToUrl(notificationLink);
        var rows = WebDriver.GetTrElements().ToArray();
        var notification = new Notification
        {
            Content = WebDriver.GetPreElement().GetContent(),
            ReceivedAt = rows.GetValue("Date").ParseToDateTime(),
            Hotel = rows.GetValue("Hotel"),
            Reference = rows.GetValue("Reference"),
            Subject = rows.GetValue("Subject")
        };
        if(Logging) Console.WriteLine($"{NotificationNumber++}) Notification");
        return Task.Run(() => notification);
    }
}