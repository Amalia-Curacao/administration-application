using TUI_Reader.Contracts;

namespace TUI_Reader.Database;

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
        Context.Notifications.AddRange(newNotifications);
        await Context.SaveChangesAsync();
        return this;
    }
    /// <summary>
    /// Gathers notifications that do not exists in the database.
    /// </summary>
    public IEnumerable<Notification> GetNewNotifications(params Notification[] notifications)
    {
        var existingNotifications = Context.Notifications.ToArray();
        foreach (var notification in notifications)
        {
            var exists = false;
            foreach (var existingNotification in existingNotifications)
            {
                if (notification.Equals(existingNotification))
                {
                    exists = true;
                }
            }
            if (!exists)
            {
                yield return notification;
            }
        }
    }

}