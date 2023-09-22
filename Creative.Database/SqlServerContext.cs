using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

public abstract class SqlServerContext : DatabaseContext
{
    public SqlServerContext(DbContextOptions options) : base(options)
    {
    }

    public SqlServerContext(string connectionString) : this(InitDbContextOptions(connectionString))
    {
    }


    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    private static DbContextOptions<SqlServerContext> InitDbContextOptions(string connectionString)
        => new DbContextOptionsBuilder<SqlServerContext>()
           .UseSqlServer(connectionString)
           .Options;
}
