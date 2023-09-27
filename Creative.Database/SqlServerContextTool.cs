using Creative.Database.Converters;
using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

public static class SqlServerContextTool
{
    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    public static DbContextOptions<TContext> InitDbContextOptions<TContext>(string connectionString) where TContext : DbContext
        => new DbContextOptionsBuilder<TContext>()
           .UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
           .Options;
        
    public static void OnConfiguring(DbContextOptionsBuilder options, string connectionString)
        => options.UseSqlServer(connectionString ?? throw new NullReferenceException("Connection string can not be null."));

    /// <summary> Configure the conventions for SQL Server. </summary>
    public static void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>();
        configurationBuilder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>();
    }
}
