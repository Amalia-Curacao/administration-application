using Microsoft.EntityFrameworkCore;

namespace TuiReader.Database;

internal class SqliteContext: DatabaseContext
{
	
	public SqliteContext() : this(InitDbContextOptions())
	{
		
	}
	public SqliteContext(DbContextOptions options) : base(options)
	{
		
	}
	private static DbContextOptions<SqliteContext> InitDbContextOptions()
		=> new DbContextOptionsBuilder<SqliteContext>()
		   .UseSqlite(DbPath)
		   .Options;
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		// Uses sqlite to make a local database.
		options.UseSqlite($"Data source={DbPath}");
	}

	
}