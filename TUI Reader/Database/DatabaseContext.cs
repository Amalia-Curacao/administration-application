using Microsoft.EntityFrameworkCore;
using TUI_Reader.Contracts;

namespace TUI_Reader.Database;

internal class DatabaseContext: DbContext
{
	/// <summary>
	/// Creates the Notifications table in the database.
	/// </summary>
	public DbSet<Notification> Notifications { get; set; } = null!;
	/// <summary>
	/// The name of the database.
	/// </summary>
	protected static string Name => "tui";
	public DatabaseContext(DbContextOptions options) : base(options)
	{
		
	}
	/// <summary>
	/// The absolute path of the database.
	/// </summary>
	/// <remarks>
	///	Assumes that the database name is in a folder called "Database".
	/// </remarks>
	protected static string DbPath => Path.Join(Path.GetFullPath("Database"), Name);
}