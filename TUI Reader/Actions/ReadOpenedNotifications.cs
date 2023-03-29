using System.Text.RegularExpressions;
using OpenQA.Selenium;
using TUI_Reader.Contracts;
using TUI_Reader.Drivers;
using TUI_Reader.Extensions;
using DriverOptions = TUI_Reader.Contracts.DriverOptions;

namespace TUI_Reader.Actions;

internal static class ReadOpenedNotifications
{
    /// <summary>
    /// Gets all the opened notifications from JIL.
    /// </summary>
    /// <returns>Opened notifications.</returns>
    public static IEnumerable<Notification> Run(ReaderOptions readerOptions)
    {
        Console.WriteLine("Read opened notifications: start");
        // The to list forces the c# to irritate through the notifications.
        var openedNotifications = GetOpenedNotifications(readerOptions).ToList();
        Console.WriteLine("Read opened notifications: successful");
        return openedNotifications;
    }
    /// <summary>
    /// Gets all the opened notifications.
    /// </summary>
    /// <returns>Opened notifications.</returns>
    private static IEnumerable<Notification> GetOpenedNotifications(ReaderOptions readerOptions)
    {
        // The to list forces the c# to irritate through the notification links.
        var openNotificationLinks = GetOpenedNotificationLinks(readerOptions.DriverOptions).ToList();
        var openedNotifications = new List<Notification>();
        var i = 0;
        Parallel.ForEach(
            openNotificationLinks,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = readerOptions.MaximumParallelOperations
            },
            openedNotificationLink =>
            {
                i++;
                Console.WriteLine($"Notification: {i}");
                openedNotifications.Add(GetNotification(readerOptions.DriverOptions, openedNotificationLink));
            });
        return openedNotifications;
    }
    /// <summary>
    /// Gets the opened notification.
    /// </summary>
    /// <returns>Opened notification.</returns>
    private static Notification GetNotification(DriverOptions driverOptions, string notificationLink)
    {
        var driver = Driver.Chrome(driverOptions);
        Login.Run(driver);
        driver.WebDriver.Navigate().GoToUrl(notificationLink);
        var rows = driver.WebDriver.GetTrElements().ToArray();
        var notification = new Notification
        {
            Content = driver.WebDriver.GetContent(),
            ReceivedAt = ParseToDateTime(GetValue(rows, "Date")),
            Hotel = GetValue(rows, "Hotel"),
            Reference = GetValue(rows, "Reference"),
            Subject = GetValue(rows, "Subject")
        };
        driver.Dispose();
        return notification;
    }
    
    /// <summary>
    /// Gets a row value.
    /// </summary>
    private static string GetValue(IEnumerable<IWebElement> rows, string rowName)
    {
        var row = FindRow(rows, rowName);
        var tdElement = GetValue(row, rowName);
        return GetInnerText(tdElement);
    }
    /// <summary>
    /// Get the innerText value from the given element.
    /// </summary>
    /// <param name="element">A HTML element.</param>
    /// <returns>InnerText value as a string</returns>
    /// <exception cref="Exception">No innerText value found in the <see cref="element"/></exception>
    private static string GetInnerText(IWebElement element) => 
        element.GetAttribute("innerText") ?? throw new Exception("No innerText value could be found in the element");
    /// <summary>
    /// Gets the child in the row element that contains the values.
    /// </summary>
    private static IWebElement GetValue(IWebElement row, string rowName)
    {
        foreach (var child in GetChildren(row))
        {
            var childValue = child.GetAttribute("innerText") ?? throw new Exception("No innerText was found in child element.");
            if(!childValue.Contains(rowName))
                return child;
        }
        throw new Exception("No \"value child\" was found");
    }
    /// <summary>
    /// Returns the element's immediate children.
    /// </summary>
    private static IEnumerable<IWebElement> GetChildren(IWebElement element)
        => element.FindElements(By.XPath("./*"));
    /// <summary>
    /// Finds a row with a child that has the same content as <see cref="rowName"/>.
    /// </summary>
    private static IWebElement FindRow(IEnumerable<IWebElement> rows, string rowName)
    {
        var foundOneRow = false;
        IWebElement? returnRow = null;
        foreach (var row in rows)
        {
            if (!HasContent(row, rowName)) continue;
            
            if (foundOneRow)
                throw new Exception($"Multiple rows found with the {rowName} as a row name.");
            
            returnRow = row;
            foundOneRow = true;
        }
        return returnRow ?? throw new Exception("Desired row could not be found.");
    }
    /// <summary>
    /// Determines if <see cref="element"/> has a child or itself has the desired content.
    /// </summary>
    /// <param name="element">HTML element.</param>
    /// <param name="content">The content you want to find.</param>
    /// <returns>If a child or the element itself has the content</returns>
    private static bool HasContent(IWebElement element, string content)
        => (element.GetAttribute("innerText") 
            ?? throw new Exception("No \"innerText found\" in element")).Contains(content) 
           || GetChildren(element).Any(child => HasContent(child, content));


    private static string GetContent(this IWebDriver driver)
        => driver.GetPreElement().GetAttribute("textContent")
           ?? throw new Exception("No text content could be found in the element");
    private static IEnumerable<IWebElement> GetTrElements(this IWebDriver driver)
        => driver.FindElements(By.TagName("tr"))
           ?? throw new Exception("No 'tr' element found on the page");
    private static IWebElement GetPreElement(this IWebDriver driver)
        => driver.FindElement(By.TagName("pre"))
           ?? throw new Exception("No pre element could be found on this page");
    
    /// <summary>
    /// Gets all the opened notifications link on the page.
    /// </summary>
    /// <returns>Opened notification links.</returns>
    /// <exception cref="Exception">No links found on the page.</exception>
    private static IEnumerable<string> GetOpenedNotificationLinks(DriverOptions driverOptions)
    {
        var driver = Driver.Chrome(driverOptions);
        Login.Run(driver);
        driver.GoToOpenedNotificationPage();
        var allPageLinks =
            driver.Wait().Until(webDriver => webDriver.FindElements(By.TagName("a")).Select(anchor => anchor.GetAttribute("href")))
            ?? throw new Exception("There were no links on the page.");
        
        var notificationLinkRegexPattern = new Regex(@"/jilhpp/messenger/viewmess/msgid/\d+/smid/1");
        var openedNotificationLinks = allPageLinks.Where(link => notificationLinkRegexPattern.Match(link).Success).ToList();
        driver.Dispose();
        return openedNotificationLinks;
    }
    
    /// <summary>
    /// Parses TUI's string date time format to <see cref="DateTime"/>.
    /// </summary>
    private static DateTime ParseToDateTime(string dateTime)
    {
        var splitDateTime = dateTime.Split('-', ' ', ':');
        var days = int.Parse(splitDateTime[0]);
        var months = int.Parse(splitDateTime[1]);
        var years = int.Parse(splitDateTime[2]);
        var hours = int.Parse(splitDateTime[3]);
        var minutes = int.Parse(splitDateTime[4]);
        var seconds = int.Parse(splitDateTime[5]);
        return new DateTime(years, months, days, hours, minutes, seconds);
    }
}