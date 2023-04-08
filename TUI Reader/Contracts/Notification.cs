﻿using Microsoft.EntityFrameworkCore;

namespace TUI_Reader.Contracts;

[PrimaryKey(nameof(Id))]
public class Notification : IEquatable<Notification>
{
	public int Id { get; init; }
    public string Reference { get; init; } = null!;
    public DateTime ReceivedAt { get; init; }
    public string Hotel { get; init; } = null!;
    public string Subject { get; init; } = null!;
    public bool LastMinute => Subject.ToLowerInvariant().Contains("last minute");
    public MessageType Type => Enum.GetValues<MessageType>()
                                   .FirstOrDefault(messageType => Subject.ToLowerInvariant().Contains(messageType.ToString().ToLowerInvariant()));
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
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Notification)obj);
	}
	/// <summary>
	/// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
	/// </summary>
	public bool Equals(Notification? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Reference == other.Reference && ReceivedAt.Equals(other.ReceivedAt) && Hotel == other.Hotel && Subject == other.Subject && Content == other.Content;
	}
	/// <summary>
	/// Generates a hashcode of the object.
	/// </summary>
	public override int GetHashCode()
	{
		return HashCode.Combine(Reference, ReceivedAt, Hotel, Subject, Content);
	}
}

public enum MessageType
{
    Undefined = 0,
    New,
    Change,
    Cancel,
    StopSale
}