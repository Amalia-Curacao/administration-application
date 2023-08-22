using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Scheduler.Database;

/// <summary> Has some helper functions for creating a <see cref="DbContext"/>. </summary>
public abstract class DatabaseContext : DbContext
{
    protected DatabaseContext(string DbName, DbContextOptions options) : base(options)
    {
        Name = DbName;
    }

    /// <summary> The name of the current project. </summary>
    private static string ProjectName => Assembly.GetExecutingAssembly().GetName().Name 
        ?? throw new InvalidOperationException("Project name is null.");


    /// <summary> The name of the database. </summary>
    protected static string Name { get; private set; } = "database";

    /// <summary> The absolute path of the database. </summary>
    /// <remarks> Assumes that the database name is in a folder called "Database". </remarks>
    protected static string DbPath
        => Path.Join(Environment.CurrentDirectory.Split(ProjectName)[0] + ProjectName, $"Database\\{Name}.db");
}
