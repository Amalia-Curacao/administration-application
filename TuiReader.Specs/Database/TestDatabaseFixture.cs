using TUI_Reader.Contracts;

namespace TuiReader.Specs.Database;

public class TestDatabaseFixture
{
    protected TestContext TestContext { get; set; }
    [SetUp]
    public void SetUp()
    {
        TestContext = TestContext.Create();
        TestContext.Database.EnsureCreated();
    }
    /// <summary>
    /// Populates the context with 3 notifications.
    /// </summary>
    protected void PopulateTestContext()
    {
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

        TestContext.Notifications.AddRange(notifications);
        TestContext.SaveChanges();
    }
    [TearDown]
    public void TearDown()
    {
        TestContext.Database.EnsureDeleted();
        TestContext.Dispose();
    }
}