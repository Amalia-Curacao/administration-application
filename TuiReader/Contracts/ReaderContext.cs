using System.Text.RegularExpressions;
using TUI_Reader.Database;
using TUI_Reader.Extensions;

namespace TUI_Reader.Contracts;

/// <summary>
/// Context used to execute the <see cref="TuiReader"/>.
/// </summary>
public class ReaderContext
{
    /// <summary>
    /// Indicates if logging should be done for the reader.
    /// </summary>
    public bool Logging { get; init; } = true;
    /// <summary>
    /// Options for creating wed drivers.
    /// </summary>
    public DriverOptions DriverOptions { get; init; } = new();
    /// <summary>
    /// The maximum amount of parallel operations.
    /// </summary>
    public int MaximumParallelOperations { get; init; } = 5;
    /// <summary>
    /// <inheritdoc cref="LoginContext"/>
    /// </summary>
    /// <remarks>
    /// If left empty will use value from the local appsettings.json.
    /// </remarks>
    public LoginContext LoginContext { get; init; } = LoginContext.DefaultLoginContext();

    /// <summary>
    /// Creates a <see cref="NotificationController"/>.
    /// </summary>
    internal async Task<NotificationController> Execute()
    {
        var notificationController = new NotificationController(new SQLiteContext());

        // The .ToArray() forces iteration through the notifications and sets it in the right list type for the add function.
        var openedNotification = (await ReadOpenedNotifications()).ToArray();

        return await notificationController.Add(openedNotification);
    }
    /// <summary>
    /// Gets all the opened notifications from JIL.
    /// </summary>
    /// <returns>Opened notifications.</returns>
    private async Task<IEnumerable<Notification>> ReadOpenedNotifications()
    {
        if (Logging) Console.WriteLine("operation: \"read notification\" started");

        var openedNotifications = await GetOpenedNotifications();

        if (Logging) Console.WriteLine("operation: \"read notification\" finished");

        return openedNotifications;
    }

    /// <summary>
    /// Gets all the opened notifications link on the page.
    /// </summary>
    /// <returns>Opened notification links.</returns>
    /// <exception cref="Exception">No links found on the page.</exception>
    private IEnumerable<string> GetOpenedNotificationLinks()
    {
        if (Logging) Console.WriteLine("operation \"get notification links\" start");
        var driver = Driver.Chrome(DriverOptions);
        driver.Login(LoginContext);
        driver.WebDriver.GoToOpenedNotificationPage();

        var allPageLinks = driver.WebDriver.GetLinkElements().GetLinkReferences();
        var notificationLinkRegexPattern = new Regex(@"/jilhpp/messenger/viewmess/msgid/\d+/smid/1");
        // The .ToList() forces iteration. 
        var openedNotificationLinks = allPageLinks.Where(link => notificationLinkRegexPattern.Match(link).Success).ToList();

        driver.Dispose();

        if (Logging) Console.WriteLine("operation \"get notification links\" completed");
        return openedNotificationLinks;
    }
    /// <summary>
    /// Gets all the opened notifications.
    /// </summary>
    /// <returns>Opened notifications.</returns>
    private async Task<IEnumerable<Notification>> GetOpenedNotifications()
    {
        // Resets the counter for notification
        var notificationCounter = 0;

        // The to list forces the c# to irritate through the notification links.
        var notificationUrls = new Queue<string>(GetOpenedNotificationLinks());

        var notifications = new List<Notification>();
        var drivers = DriverOptions.Create(MaximumParallelOperations).Login(LoginContext).ToArray();

        await Parallel.ForEachAsync(
            drivers,
            new ParallelOptions { MaxDegreeOfParallelism = MaximumParallelOperations },
            async (driver, _) =>
            {
                while (notificationUrls.Any())
                {
                    if (!notificationUrls.TryDequeue(out var url)) continue;
                    if (string.IsNullOrWhiteSpace(url)) continue;
                    if (Logging) Console.WriteLine($"Notification: {notificationCounter++}");
                    notifications.Add(await driver.GetNotification(url));
                }
            });


        drivers.Dispose();

        return notifications;
    }

}