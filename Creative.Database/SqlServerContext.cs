using Creative.Database.Converters;
using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

public abstract class SqlServerContext : DatabaseContext
{
    private static string? _connectionString;
    public SqlServerContext(DbContextOptions options) : base(options)
    {
    }

    public SqlServerContext(string connectionString) : this(InitDbContextOptions(connectionString))
    {
        _connectionString = connectionString;
    }


    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    private static DbContextOptions<SqlServerContext> InitDbContextOptions(string connectionString)
        => new DbContextOptionsBuilder<SqlServerContext>()
           .UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
           .Options;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(_connectionString ?? throw new NullReferenceException("Connection string can not be null."));

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>();
        configurationBuilder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>();
    }
}
