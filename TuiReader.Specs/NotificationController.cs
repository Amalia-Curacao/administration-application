using TuiReader.Contracts;
using TuiReader.Database;
using TuiReader.Specs.Notifications.Database;

namespace TuiReader.Specs.Notification_controller;

[TestFixture]
public class Can_detect : TestDatabaseFixture
{
	[Test]
	public void duplicate_notifications()
	{
		PopulateTestContext();
		var controller = new NotificationController(TestContext);
		var notifications = controller.ReadAll().ToArray();
		var newNotifications = controller.GetNewNotifications(notifications.ToArray());

		newNotifications.Should().HaveCount(0);
	}

	[Test]
	public void non_duplicate_notifications()
	{
		PopulateTestContext();
		var controller = new NotificationController(TestContext);
		var notifications = controller.ReadAll().ToList();
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
		var notifications = controller.ReadAll().ToArray();
		await controller.Add(notifications);
		
		controller.ReadAll().Should().HaveCount(notifications.Length).And.OnlyHaveUniqueItems();
		
	}
	
	[Test]
	public async Task only_non_duplicate_notifications()
	{
		PopulateTestContext();
		var controller = new NotificationController(TestContext);
		var notifications = controller.ReadAll().ToList();
		var originalCount = notifications.Count;
		var newNotification = new Notification[]
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
		
		notifications.AddRange(newNotification);
		await controller.Add(notifications.ToArray());

		controller.ReadAll().Should().HaveCount(originalCount + newNotification.Length).And.OnlyHaveUniqueItems();
		
	}
}