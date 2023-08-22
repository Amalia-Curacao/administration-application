using Microsoft.EntityFrameworkCore;

namespace TUI_Reader.Database;
/// <summary>
/// <see cref="DatabaseContext"/> that uses SQLite.
/// </summary>
internal class SQLiteContext : DatabaseContext
{
    /// <summary>
    /// Default constructor for <see cref="SQLiteContext"/>.
    /// </summary>
    public SQLiteContext() : this(InitDbContextOptions()) { }
    /// <summary>
    /// Constructor for the <see cref="SQLiteContext"/>.
    /// </summary>
    public SQLiteContext(DbContextOptions options) : base(options) { }
    /// <summary>
    /// Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SQLiteContext"/>.
    /// </summary>
    private static DbContextOptions<SQLiteContext> InitDbContextOptions()
        => new DbContextOptionsBuilder<SQLiteContext>()
           .UseSqlite(DbPath)
           .Options;
    /// <summary>
    /// <inheritdoc cref="OnConfiguring"/>
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data source={DbPath}");


}