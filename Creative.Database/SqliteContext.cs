using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

/// <summary> <see cref="DatabaseContext"/> that uses SQLite. </summary>
public abstract class SqliteContext : DatabaseContext
{   
    /// <summary> Constructor for the <see cref="SqliteContext"/>. </summary>
    public SqliteContext(string DbName, DbContextOptions options) : base(DbName, options) { }

    /// <summary> Default constructor for <see cref="SqliteContext"/>. </summary>
    public SqliteContext(string DbName) : this(DbName, InitDbContextOptions()) { }

    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    private static DbContextOptions<SqliteContext> InitDbContextOptions()
        => new DbContextOptionsBuilder<SqliteContext>()
           .UseSqlite(DbPath)
           .Options;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data source={DbPath}");


}
