using Creative.Database;
using Creative.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Creative.Test.Utils;

public abstract class MockDatabase
{

    protected static TestContext _dbContext = new TestContext();

    public class Key : IKey<int>
    {
        public Key(int value) { Value = value; }
        public int Value { get; init; }
        public int CompareTo(object? obj)
        {
            if(obj is Key other) return Value.CompareTo(other.Value);
            throw new Exception($"Object is not of type: {typeof(Key)}");
        }
        int[]? IKey<int>.DbKeyCollection() => new int[] { Value };
        public static implicit operator int(Key key) => key.Value;
        public static implicit operator Key(int i) => new(i);
    }



    [PrimaryKey(nameof(PrimaryKey))]
    public class SimpleObject
    {
        public required int PrimaryKey { get; init; }
        public string? Value { get; set; }
    }

    public class TestContext : InMemoryContext
    {
        public TestContext(DbContextOptions options) : base(Guid.NewGuid().ToString(), options) 
        {
            
        }
        public TestContext() : base(Guid.NewGuid().ToString()) { }
        public DbSet<SimpleObject> Objects { get; set; }
    }

    [SetUp]
    public void Init()
    {
        _dbContext = new TestContext();
        _dbContext.Database.EnsureCreated();
        _dbContext.AddRange(seed_database());
        _dbContext.SaveChanges();
    }

    public static SimpleObject[] seed_database()
        => new[] {
                new SimpleObject{ PrimaryKey = new Key(1), Value="value" },
                new SimpleObject{ PrimaryKey = new Key(2) , Value="value" },
        };


    [TearDown]
    [OneTimeTearDown]
    public void Cleanup()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
