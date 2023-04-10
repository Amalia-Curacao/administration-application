using Microsoft.EntityFrameworkCore;
using TUI_Reader.Contracts;

namespace TUI_Reader.Database;

public class NotificationController
{
	private SqliteContext SqliteContext { get; }
	internal NotificationController(SqliteContext sqliteContext)
	{
		SqliteContext = sqliteContext;
	}
	/// <summary>
	/// Checks if given notification exists in the database.
	/// </summary>
	/// <returns>Existence of <see cref="Notification"/> in the database.</returns>
	public async Task<bool> ExistsAsync(Notification notification)
	=> await SqliteContext.Notifications.AnyAsync(n => n.Equals(notification));
	/// <summary>
	/// Gets all notifications from the database.
	/// </summary>
	/// <returns>All notifications from the database</returns>
	public IEnumerable<Notification> ReadAll()
		=> SqliteContext.Notifications;
	/// <summary>
	/// Adds notifications to the database.
	/// </summary>
	/// <param name="notifications">Notifications to add to the database.</param>
	/// <param name="overrideExisting">If received notifications should override existing one in the database.</param>
	public async Task<NotificationController> Add(bool overrideExisting, params Notification[] notifications)
	{
		var tasks = notifications.Select(notification => Task.Run(() => Add(overrideExisting, notification)));
		await Task.WhenAll(tasks);
		return this;
	}
	/// <summary>
	/// Adds a notification to the database.
	/// </summary>
	/// <param name="overrideExisting">If received notification should override existing one in the database.</param>
	/// <param name="notification">Notification to add to the database.</param>
	public async Task<NotificationController> Add(bool overrideExisting, Notification notification)
	{
		if (!overrideExisting) await SqliteContext.Notifications.AddAsync(notification);
		else
		{
			if (await ExistsAsync(notification))
				SqliteContext.Notifications.Update(notification);
			else
				await SqliteContext.Notifications.AddAsync(notification);
			
		}
		await SqliteContext.SaveChangesAsync();
		return this;
	}
}