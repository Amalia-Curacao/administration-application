using System.Text.RegularExpressions;
using OpenQA.Selenium;
using TuiReader.WebElements;
using TuiReader.Database;

namespace TuiReader.Contracts;

public static class ReaderContextExtensions
{
	public async static Task<NotificationController> Execute(this ReaderContext context)
	{
		var notificationController = new NotificationController(new SqliteContext());
		
		// The .ToArray() forces iteration through the notifications and sets it in the right list type for the add function.
		var openedNotification = (await context.ReadOpenedNotifications()).ToArray();
		
		return await notificationController.Add(true, openedNotification);
	}
	/// <summary>
	/// Gets all the opened notifications from JIL.
	/// </summary>
	/// <returns>Opened notifications.</returns>
	internal async static Task<IEnumerable<Notification>> ReadOpenedNotifications(this ReaderContext context)
	{
		if (context.Logging) Console.WriteLine("operation: \"read notification\" started");
        
		var openedNotifications = await context.GetOpenedNotifications();
        
		if (context.Logging) Console.WriteLine("operation: \"read notification\" finished");
        
		return openedNotifications;
	}
	
	/// <summary>
	/// Gets all the opened notifications link on the page.
	/// </summary>
	/// <returns>Opened notification links.</returns>
	/// <exception cref="Exception">No links found on the page.</exception>
	internal static IEnumerable<string> GetOpenedNotificationLinks(this ReaderContext context)
	{
		if(context.Logging) Console.WriteLine("operation \"get notification links\" start");
		var driver = Driver.Chrome(context.DriverOptions);
		driver.Login(context.LoginContext);
		driver.WebDriver.GoToOpenedNotificationPage();
        
		var allPageLinks = driver.WebDriver.GetLinkElements().GetLinkReferences();
		var notificationLinkRegexPattern = new Regex(@"/jilhpp/messenger/viewmess/msgid/\d+/smid/1");
		// The .ToList() forces iteration. 
		var openedNotificationLinks = allPageLinks.Where(link => notificationLinkRegexPattern.Match(link).Success).ToList();
        
		driver.Dispose();
        
		if(context.Logging) Console.WriteLine("operation \"get notification links\" completed");
		return openedNotificationLinks;
	}
	/// <summary>
	/// Gets all the opened notifications.
	/// </summary>
	/// <returns>Opened notifications.</returns>
	internal async static Task<IEnumerable<Notification>> GetOpenedNotifications(this ReaderContext context)
	{
		// Resets the counter for notification
		var notificationCounter = 0;

		// The to list forces the c# to irritate through the notification links.
		var notificationUrls = new Queue<string>(context.GetOpenedNotificationLinks().Take(1));
		var notifications = new List<Notification>();
		var drivers = new List<Driver>();
		for (var i = 0; i < context.MaximumParallelOperations; i++)
		{
			drivers.Add(Driver.Chrome(context.DriverOptions).Login(context.LoginContext));
		}

		await Parallel.ForEachAsync(
			drivers,
			new ParallelOptions { MaxDegreeOfParallelism = context.MaximumParallelOperations },
			async (driver, _) =>
			{
				while (notificationUrls.Any())
				{
					string url;
					try
					{
						url = notificationUrls.Dequeue();
						if (string.IsNullOrEmpty(url)) break;
					}
					catch
					{
						break;
					}
					if (context.Logging) Console.WriteLine($"Notification: {notificationCounter++}");
					notifications.Add(await driver.GetNotification(url));
				}
			});
		
		
		drivers.ForEach(driver => driver.Dispose());
		
		return notifications;
	}
	
}