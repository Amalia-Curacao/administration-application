using Microsoft.EntityFrameworkCore;

namespace TuiReader.Database;
/// <summary>
/// <see cref="DatabaseContext"/> that uses sqlite.
/// </summary>
internal class SqliteContext: DatabaseContext
{
	/// <summary>
	/// Default constructor for <see cref="SqliteContext"/>.
	/// </summary>
	public SqliteContext() : this(InitDbContextOptions()) { }
	/// <summary>
	/// Constructor for the <see cref="SqliteContext"/>.
	/// </summary>
	public SqliteContext(DbContextOptions options) : base(options) { }
	/// <summary>
	/// Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>.
	/// </summary>
	private static DbContextOptions<SqliteContext> InitDbContextOptions()
		=> new DbContextOptionsBuilder<SqliteContext>()
		   .UseSqlite(DbPath)
		   .Options;
	/// <summary>
	/// <inheritdoc cref="OnConfiguring"/>
	/// </summary>
	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data source={DbPath}");


}