using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

/// <summary> <see cref="DatabaseContext"/> that uses SQLite. </summary>
public static class SqliteContextTool
{   
    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    public static DbContextOptions<TContext> InitDbContextOptions<TContext>(string path) where TContext : DbContext
        => new DbContextOptionsBuilder<TContext>()
           .UseSqlite(path)
           .Options;

    /// <summary> Sets the configuration of the context to use SQLite. </summary>
    /// <param name="path"> The path to the SQLite database. </param>
    public static void OnConfiguring(DbContextOptionsBuilder options, string path)
        => options.UseSqlite($"Data source={path}");
}
