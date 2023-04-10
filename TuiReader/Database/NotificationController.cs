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
	private SqliteContext Context { get; }
	/// <summary>
	/// Constructor for the <see cref="NotificationController"/>.
	/// </summary>
	internal NotificationController(SqliteContext context)
	{
		Context = context;
		Context.Database.EnsureCreated();
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
		if (overrideExisting)
		{
			var existingNotifications = await GetExisting(notifications);
			var nonExistingNotifications = await GetNonExisting(notifications);
			Context.Notifications.UpdateRange(existingNotifications);
			await Context.Notifications.AddRangeAsync(nonExistingNotifications);
		}
		else
			await Context.Notifications.AddRangeAsync(notifications);
		await Context.SaveChangesAsync();
		return this;
	}
	/// <summary>
	/// Filters the only notifications that exist in the database.
	/// </summary>
	private async Task<IEnumerable<Notification>> GetExisting(IEnumerable<Notification> notifications)
	{
		var existing = new List<Notification>();

		foreach (var notification in notifications)
		{
			if (await ExistsAsync(notification))
			{
				existing.Add(notification);
			}
		}
		
		return existing;
	}
	
	/// <summary>
	/// Filters the only notifications that do not exist in the database.
	/// </summary>
	private async Task<IEnumerable<Notification>> GetNonExisting(IEnumerable<Notification> notifications)
	{
		var nonExisting = new List<Notification>();

		foreach (var notification in notifications)
		{
			if (!await ExistsAsync(notification))
			{
				nonExisting.Add(notification);
			}
		}
		
		return nonExisting;
	}
}