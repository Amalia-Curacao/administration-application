using Creative.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

public abstract class InMemoryContext : DatabaseContext
{
    public InMemoryContext(DbContextOptions options) : base(options) { }

    /// <summary> Default constructor for <see cref="SqliteContext"/>. </summary>
    public InMemoryContext(DatabaseContextOptions options) : base(InitDbContextOptions(options)) { }

    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    private static DatabaseContextOptions InitDbContextOptions(DatabaseContextOptions options)
    {
        options.DbOptions = new DbContextOptionsBuilder<InMemoryContext>().UseInMemoryDatabase(DbPath()).Options;
        return options;
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase(Options.DbName);
}
