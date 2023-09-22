using Microsoft.EntityFrameworkCore;

namespace Creative.Database;

public abstract class InMemoryContext : DatabaseContext
{
    public InMemoryContext(string DbName, DbContextOptions options) : base(DbName, options)
    {
    }

    /// <summary> Default constructor for <see cref="SqliteContext"/>. </summary>
    public InMemoryContext(string DbName) : this(DbName, InitDbContextOptions()) { }

    /// <summary> Initialize <see cref="DbContextOptions{TContext}"/> for <see cref="SqliteContext"/>. </summary>
    private static DbContextOptions<InMemoryContext> InitDbContextOptions()
        => new DbContextOptionsBuilder<InMemoryContext>()
           .UseInMemoryDatabase(DbPath)
           .Options;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase(Name);
}
