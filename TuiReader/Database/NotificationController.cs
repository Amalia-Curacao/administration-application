using Microsoft.EntityFrameworkCore;
using TuiReader.Contracts;

namespace TuiReader.Database;

/// <summary>
/// Controller for the notifications in the database.
/// </summary>
public class NotificationController
{
	/// <summary>
	/// Context for the database.
	/// </summary>
	private DatabaseContext Context { get; }
	/// <summary>
	/// Constructor for the <see cref="NotificationController"/>.
	/// </summary>
	internal NotificationController(DatabaseContext context)
	{
		Context = context;
	}
	/// <summary>
	/// Checks if given notification exists in the database.
	/// </summary>
	/// <returns>Existence of <see cref="Notification"/> in the database.</returns>
	public async Task<bool> ExistsAsync(Notification notification)
	=> await Context.Notifications.AnyAsync(n => n.Equals(notification));
	/// <summary>
	/// Gets all notifications from the database.
	/// </summary>
	/// <returns>All notifications from the database</returns>
	public IEnumerable<Notification> ReadAll()
		=> Context.Notifications;
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
		if (!overrideExisting) await Context.Notifications.AddAsync(notification);
		else
		{
			if (await ExistsAsync(notification))
				Context.Notifications.Update(notification);
			else
				await Context.Notifications.AddAsync(notification);
			
		}
		await Context.SaveChangesAsync();
		return this;
	}
}