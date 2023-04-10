using Microsoft.EntityFrameworkCore;

namespace TuiReader.Contracts;

/// <summary>
/// Notification sent from TUI.
/// </summary>
[PrimaryKey(nameof(Id))]
public class Notification : IEquatable<Notification>
{
	/// <summary>
	/// Internal id number used in the database.
	/// </summary>
	internal int Id { get; init; }
	/// <summary>
	/// Reference code that TUI uses for tracking the notification.
	/// </summary>
    public string Reference { get; init; } = null!;
	/// <summary>
	/// The <see cref="DateTime">date and time</see> that the notification is received at. 
	/// </summary>
    public DateTime ReceivedAt { get; init; }
	/// <summary>
	/// Name of the hotel the notification is for.
	/// </summary>
    public string Hotel { get; init; } = null!;
	/// <summary>
	/// Subject of the notification.
	/// </summary>
    public string Subject { get; init; } = null!;
	/// <summary>
	/// Indicates if the notification is pushed last minute.
	/// </summary>
    public bool LastMinute => Subject.ToLowerInvariant().Contains("last minute");
	/// <summary>
	/// <see cref="NotificationType"/>
	/// </summary>
    public NotificationType Type => Enum.GetValues<NotificationType>()
                                   .FirstOrDefault(messageType => Subject.ToLowerInvariant().Contains(messageType.ToString().ToLowerInvariant()));
	/// <summary>
	/// Content found in the notification.
	/// </summary>
    public string? Content { get; init; }
	/// <summary>
	/// Generates a string from the object's values.
	/// </summary>
    public override string ToString()
        => $"Reference: {Reference}" + Environment.NewLine +
           $"Received at: {ReceivedAt}" + Environment.NewLine +
           $"Hotel: {Hotel}" + Environment.NewLine +
           $"Subject: {Subject}" + (LastMinute ? "Last minute" : "") + Environment.NewLine +
           $"Type: {Type.ToString()}" + Environment.NewLine +
           Environment.NewLine +
           $"Content: {Content}";

	/// <summary>
	/// Indicates whether the current object is equal to another object.
	/// </summary>
	public override bool Equals(object? obj)
	{
		if(obj is Notification other)
		{
			return this.Equals(other);
		}
		return false;
	}
	/// <summary>
	/// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
	/// </summary>
	public bool Equals(Notification? other)
	{
		if (other is null) return false;
		return Reference == other.Reference &&
			   ReceivedAt.Equals(other.ReceivedAt) &&
			   Hotel == other.Hotel &&
			   Subject == other.Subject &&
			   LastMinute == other.LastMinute &&
			   Type == other.Type &&
			   Content == other.Content;
	}
	/// <summary>
	/// Generates a hashcode of the object.
	/// </summary>
	public override int GetHashCode()
	{
		return HashCode.Combine(Reference, ReceivedAt, Hotel, Subject, Content);
	}
}

/// <summary>
/// Type of notification.
/// </summary>
public enum NotificationType
{
	/// <summary>
	/// Undefined notification type.
	/// </summary>
	/// <remarks>
	///	This type should be used as a last resort when a notification type is yet to be defined.
	/// </remarks>
    Undefined = 0,
	/// <summary>
	/// Received a new reservation.
	/// </summary>
    New,
	/// <summary>
	/// Reservation has been changed.
	/// </summary>
    Change,
	/// <summary>
	/// Reservation has been canceled.
	/// </summary>
    Cancel,
	/// <summary>
	/// A stop sale has been executed for a specific time.
	/// </summary>
    StopSale
}