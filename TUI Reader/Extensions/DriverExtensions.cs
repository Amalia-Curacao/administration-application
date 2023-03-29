using OpenQA.Selenium.Support.UI;
using TUI_Reader.Drivers;

namespace TUI_Reader.Extensions;

/// <summary>
/// Extensions for <see cref="Driver"/>.
/// </summary>
internal static class DriverExtensions
{
    /// <summary>
    /// Goes to a page with all of the read notifications.
    /// </summary>
    public static void GoToOpenedNotificationPage(this Driver driver) 
        => driver.WebDriver.Navigate().GoToUrl(@"https://www.jil.travel/jilhpp/messenger/read/page/0/sm/1/generalproductid/32185");
    /// <summary>
    /// Creates a <see cref="WebDriverWait"/> with a 5 second wait time.
    /// </summary>
    /// <param name="seconds">Amount of seconds to wait.</param>
    /// <returns></returns>
    public static WebDriverWait Wait(this Driver driver, int seconds = 5) => new (driver.WebDriver, TimeSpan.FromSeconds(seconds));

    
    
}