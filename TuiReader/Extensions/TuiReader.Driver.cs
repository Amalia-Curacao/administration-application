using System.Data;
using OpenQA.Selenium;
using TuiReader.WebElements;
using TuiReader.Contracts;

namespace TuiReader;

/// <summary>
/// Extensions for <see cref="Driver"/>.
/// </summary>
internal static class DriverExtensions
{
    /// <summary>
    /// Gets used for logging to keep track of notifications read.
    /// </summary>
    private static int NotificationNumber;
    /// <summary>
    /// Login to JILL's website.
    /// </summary>
    public static Driver Login(this Driver driver, LoginContext context)
    {
        if(driver.Logging) Console.WriteLine($"{driver.Id}: login start");
        
        driver.LoggedIn = AttemptLogin(driver, context);
        
        if (!driver.LoggedIn) throw new Exception($"{driver.Id}: login failed");
        
        if (driver.Logging) Console.WriteLine($"{driver.Id}: login successful");
        return driver;
    }
    /// <summary>
    /// Attempt login to JILL's website.
    /// </summary>
    /// <returns>
    /// Is true if login was successful.
    /// </returns>
    private static bool AttemptLogin(this Driver driver, LoginContext context)
    {
        var webDriver = driver.WebDriver;
        webDriver.GoToLoginPage();
        // Will attempt to login 20 times before giving up. This is added because TUI has a shit login system.
        for (var i = 0; i < 20; i++)
        {
            webDriver.ClearCredentials();
            webDriver.FillCredentials(context);
            webDriver.GetLoginButton().Click();
            try
            {
                return webDriver.Wait().Until(wd => wd.IsOnHomePage());
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
    public static Task<Notification> GetNotification(this Driver driver, string notificationLink)
    {
        if (string.IsNullOrEmpty(notificationLink)) throw new NoNullAllowedException($"{nameof(notificationLink)} parameter cannot be null or empty.");
        var webDriver = driver.WebDriver;
        webDriver.Navigate().GoToUrl(notificationLink);
        var rows = webDriver.GetTrElements().ToArray();
        var notification = new Notification
        {
            Content = webDriver.GetPreElement().GetContent(),
            ReceivedAt = rows.GetValue("Date").ParseToDateTime(),
            Hotel = rows.GetValue("Hotel"),
            Reference = rows.GetValue("Reference"),
            Subject = rows.GetValue("Subject")
        };
        if(driver.Logging) Console.WriteLine($"{NotificationNumber++}) Notification");
        return Task.Run(() => notification);
    }
}