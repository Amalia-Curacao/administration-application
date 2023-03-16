namespace TUI_Reader.Contracts;

public class Notification
{
    public string Reference { get; init; } = null!;
    public DateTime ReceivedAt { get; init; }
    public string Hotel { get; init; } = null!;
    public string Subject { get; init; } = null!;
    public bool LastMinute => Subject.ToLowerInvariant().Contains("last minute");
    public MessageType Type => Enum.GetValues<MessageType>()
                                   .FirstOrDefault(messageType => Subject.ToLowerInvariant().Contains(messageType.ToString().ToLowerInvariant()));
    public string? Content { get; init; }
    public override string ToString()
        => $"Reference: {Reference}" + Environment.NewLine +
           $"Received at: {ReceivedAt}" + Environment.NewLine +
           $"Hotel: {Hotel}" + Environment.NewLine +
           $"Subject: {Subject}" + (LastMinute ? "Last minute" : "") + Environment.NewLine +
           $"Type: {Type.ToString()}" + Environment.NewLine +
           Environment.NewLine +
           $"Content: {Content}";
}

public enum MessageType
{
    Undefined = 0,
    New,
    Change,
    Cancel,
    StopSale
}