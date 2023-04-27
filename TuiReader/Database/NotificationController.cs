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
	public NotificationController(DatabaseContext context)
	{
		Context = context;
		Context.Database.EnsureCreated();
	}
	/// <summary>
	/// Checks if given notification exists in the database.
	/// </summary>
	/// <returns>Existence of <see cref="Notification"/> in the database.</returns>
	public bool Exists(Notification notification)
	=> Enumerable.Any(Context.Notifications, existingNotification => notification.Equals(existingNotification));
	/// <summary>
	/// Gets all notifications from the database.
	/// </summary>
	/// <returns>All notifications from the database.</returns>
	public IEnumerable<Notification> ReadAll()
		=> Context.Notifications;
	/// <summary>
	/// Adds only new notifications to the database.
	/// </summary>
	/// <param name="notifications">Notifications to add to the database.</param>
	public async Task<NotificationController> Add(params Notification[] notifications)
	{
		var newNotifications = GetNewNotifications(notifications);
		await Context.Notifications.AddRangeAsync(newNotifications);
		await Context.SaveChangesAsync();
		return this;
	}

	public IEnumerable<Notification> GetNewNotifications(params Notification[] notifications)
		=> notifications.Where(notification => !Exists(notification));
}