using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TUI_Reader.Contracts;

namespace TUI_Reader.Database;

/// <summary>
/// Database context used for the <see cref="TuiReader"/>.
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// The name of the current project.
    /// </summary>
    private static string ProjectName => Assembly.GetExecutingAssembly().GetName().Name ?? throw new InvalidOperationException("Project name is null.");
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
    protected static string DbPath
        => Path.Join(Environment.CurrentDirectory.Split(ProjectName)[0] + ProjectName, $"Database\\{Name}.db");
}