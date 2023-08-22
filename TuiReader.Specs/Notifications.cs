using TUI_Reader.Contracts;

namespace TuiReader.Specs;

public class Can_be_compared_to_a
{

    [Test]
    public void notification_and_returns_true_if_they_are_the_same_except_for_the_id()
    {
        const string subject = "New Last Minute";
        const string reference = "A12B23C45";
        const string hotel = "Amalia";
        const string content = "content";
        var recievedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 09, minute: 01, second: 01);

        var notification = new Notification
        {
            Content = content,
            Hotel = hotel,
            Reference = reference,
            Subject = subject,
            ReceivedAt = recievedAt,
        };
        var otherNotification = new Notification
        {
            Content = content,
            Hotel = hotel,
            Reference = reference,
            Subject = subject,
            ReceivedAt = recievedAt,
        };

        notification.Equals(otherNotification).Should().BeTrue();
    }

    [Test]
    public void notification_and_returns_false_if_they_are_not_the_same_except_for_the_ids()
    {
        var notification = new Notification
        {
            Content = "content",
            Hotel = "Amalia",
            Reference = "A12B23C45",
            Subject = "New Last Minute",
            ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 09, minute: 01, second: 01),
        };
        var otherNotification = new Notification
        {
            Content = "not the same content",
            Hotel = "Other hotel",
            Reference = "A98B76C54",
            Subject = "Change",
            ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 09, minute: 01, second: 02),
        };

        notification.Equals(otherNotification).Should().BeFalse();
    }
}