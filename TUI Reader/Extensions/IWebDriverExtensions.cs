using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TUI_Reader.Extensions;

internal static class IWebDriverExtensions
{
    /// <summary>
    /// Checks if the driver is currently at the home page.
    /// </summary>
    public static bool IsOnHomePage(this IWebDriver driver) 
        => driver.Title.Equals(@"JIL Hotel Partner Platform");
    /// <summary>
    /// Goes to JIL's login page.
    /// </summary>
    public static void GoToLoginPage(this IWebDriver driver) 
        => driver.Navigate().GoToUrl(@"https://www.jil.travel/");
    /// <summary>
    /// Goes to a page with all of the read notifications.
    /// </summary>
    public static void GoToOpenedNotificationPage(this IWebDriver driver) 
        => driver.Navigate().GoToUrl(@"https://www.jil.travel/jilhpp/messenger/read/page/0/sm/1/generalproductid/32185");
    /// <summary>
    /// Creates a <see cref="WebDriverWait"/> with a 5 second wait time.
    /// </summary>
    public static WebDriverWait Wait(this IWebDriver driver) => new (driver, TimeSpan.FromSeconds(5));

}