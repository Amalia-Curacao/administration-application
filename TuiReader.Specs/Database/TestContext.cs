using Microsoft.EntityFrameworkCore;
using TuiReader.Database;

namespace TuiReader.Specs.Notifications.Database;

public class TestContext : DatabaseContext
{
	private Guid Guid { get; init; }
	protected TestContext(DbContextOptions options) : base(options) { }
	public static TestContext Create()
	{
		var guid = Guid.NewGuid();
		var options = InitDbContextOptions(guid);
		return new TestContext(options)
		{
			Guid = guid
		};
	}
	private static DbContextOptions InitDbContextOptions(Guid guid)
		=> new DbContextOptionsBuilder<TestContext>()
		   .UseInMemoryDatabase(guid.ToString())
		   .Options;
}