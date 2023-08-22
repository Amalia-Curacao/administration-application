using TUI_Reader.Contracts;
using TUI_Reader.Database;
using TuiReader.Specs.Database;

namespace TuiReader.Specs;

[TestFixture]
public class Can_detect : TestDatabaseFixture
{
    [Test]
    public void duplicate_notifications()
    {
        PopulateTestContext();
        var controller = new NotificationController(TestContext);
        var notifications = new Notification[]
        {
            new()
            {
                Content = "content1",
                Reference = "reference1",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel1",
                Subject = "subject1"
            },
            new()
            {
                Content = "content2",
                Reference = "reference2",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel2",
                Subject = "subject2"
            },
            new()
            {
                Content = "content3",
                Reference = "reference3",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel3",
                Subject = "subject3"
            },
        };
        var newNotifications = controller.GetNewNotifications(notifications.ToArray());

        newNotifications.Should().HaveCount(0);
    }

    [Test]
    public void non_duplicate_notifications()
    {
        PopulateTestContext();
        var controller = new NotificationController(TestContext);
        var notifications = new List<Notification>()
        {
            new()
            {
                Content = "content1",
                Reference = "reference1",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel1",
                Subject = "subject1"
            },
            new()
            {
                Content = "content2",
                Reference = "reference2",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel2",
                Subject = "subject2"
            },
            new()
            {
                Content = "content3",
                Reference = "reference3",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel3",
                Subject = "subject3"
            },
        };
        var newNotification = new Notification
        {
            Content = "unique content",
            Hotel = "unique hotel",
            Reference = "unique reference",
            Subject = "unique subject",
            ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 10, minute: 27, second: 01)
        };
        notifications.Add(newNotification);
        var newNotifications = controller.GetNewNotifications(notifications.ToArray());

        newNotifications.Should().Equal(newNotification).And.HaveCount(1);
    }
}

[TestFixture]
public class Saves_notifications_in_the_database : TestDatabaseFixture
{
    [Test]
    public async Task while_ignoring_duplicates()
    {
        PopulateTestContext();
        var controller = new NotificationController(TestContext);
        var notifications = new Notification[]
        {
            new()
            {
                Content = "content1",
                Reference = "reference1",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel1",
                Subject = "subject1"
            },
            new()
            {
                Content = "content2",
                Reference = "reference2",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel2",
                Subject = "subject2"
            },
            new()
            {
                Content = "content3",
                Reference = "reference3",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel3",
                Subject = "subject3"
            },
        };
        await controller.Add(notifications);

        controller.ReadAll().Should().HaveCount(notifications.Length).And.OnlyHaveUniqueItems();

    }

    [Test]
    public async Task only_non_duplicate_notifications()
    {
        PopulateTestContext();
        var controller = new NotificationController(TestContext);
        var notifications = new List<Notification>
        {
            new()
            {
                Content = "content1",
                Reference = "reference1",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel1",
                Subject = "subject1"
            },
            new()
            {
                Content = "content2",
                Reference = "reference2",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel2",
                Subject = "subject2"
            },
            new()
            {
                Content = "content3",
                Reference = "reference3",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 11, minute: 08, second: 01),
                Hotel = "hotel3",
                Subject = "subject3"
            },
        };
        var originalCount = notifications.Count;
        var newNotifications = new Notification[]
        {
            new()
            {
                Content = "unique content1",
                Hotel = "unique hotel1",
                Reference = "unique reference1",
                Subject = "unique subject1",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 10, minute: 27, second: 01)
            },
            new()
            {
                Content = "unique content2",
                Hotel = "unique hotel2",
                Reference = "unique reference2",
                Subject = "unique subject2",
                ReceivedAt = new DateTime(year: 2023, month: 04, day: 27, hour: 10, minute: 27, second: 01)
            },
        };

        notifications.AddRange(newNotifications);
        await controller.Add(notifications.ToArray());

        controller.ReadAll().Should().HaveCount(originalCount + newNotifications.Length).And.OnlyHaveUniqueItems();

    }
}