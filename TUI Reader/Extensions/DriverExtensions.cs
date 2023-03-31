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
    /// Creates a <see cref="WebDriverWait"/> with a default wait time of 5 second.
    /// </summary>
    public static WebDriverWait Wait(this Driver driver, TimeSpan? time = null) => new (driver.WebDriver, time ?? TimeSpan.FromSeconds(5));

    
    
}