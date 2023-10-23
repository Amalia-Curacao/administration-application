using Creative.Api.Data;
using Creative.Api.Exceptions;
using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CrudTests;

[TestFixture]
public class Read
{
    private DbContextOptions<DbContext> _dbContextOptions;
    private DbContext _dbContext;
    private Crud<TestModel> _crud;

    [SetUp]
    public void SetUp()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DbContext>()
		   .UseInMemoryDatabase(Guid.NewGuid().ToString())
		   .Options;
		_dbContext = new TestContext(_dbContextOptions);
        _crud = new Crud<TestModel>(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Add_ShouldAddObjectToDatabase()
    {
        // Arrange
        var testModel = new TestModel { Id = 1, Name = "Test" };

        // Act
        var result = await _crud.Add(objs: testModel);

        // Assert
        result.Should().HaveCount(1);
        result.First().Should().BeEquivalentTo(testModel);
        _dbContext.Set<TestModel>().Should().Contain(testModel);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllObjectsFromDatabase()
    {
        // Arrange
        var testModel1 = new TestModel { Id = 1, Name = "Test1" };
        var testModel2 = new TestModel { Id = 2, Name = "Test2" };
        await _crud.Add(true, testModel1, testModel2);

        // Act
        var result = await _crud.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(testModel1);
        result.Should().Contain(testModel2);
    }

    [Test]
    public async Task TryGet_ShouldReturnObjectWithSpecifiedPrimaryKey()
    {
        // Arrange
        var testModel1 = new TestModel { Id = 1, Name = "Test1" };
        var testModel2 = new TestModel { Id = 2, Name = "Test2" };
        await _crud.Add(true, testModel1, testModel2);

        // Act
        var result = _crud.TryGet(new HashSet<Key> { new Key("Id", 2) });

        // Assert
        result.Should().BeEquivalentTo(testModel2);
    }

    [Test]
    public async Task Get_ShouldThrowObjectNotFoundException_WhenObjectWithSpecifiedPrimaryKeyDoesNotExist()
    {
        // Arrange
        var testModel1 = new TestModel { Id = 1, Name = "Test1" };
        var testModel2 = new TestModel { Id = 2, Name = "Test2" };
        await _crud.Add(true, testModel1, testModel2);

        // Act
        var act = () => _crud.Get(new HashSet<Key> { new Key("Id", 3) });

        // Assert
        act.Should().Throw<ObjectNotFoundException>();
    }

    [Test]
    public async Task GetLazy_ShouldReturnObjectWithSpecifiedPrimaryKeyWithoutLoadingRelatedEntities()
    {
        // Arrange
        var relationEntity = new RelatedEntity { Id = 1, Name = "Test" };
        var testModel1 = new TestModel { Id = 1, Name = "Test1", RelatedEntities = new[] { relationEntity }};
        var expected = new TestModel { Id = 1, Name = "Test1"};
        await _crud.Add(true, testModel1);

        // Act
        var result = _crud.GetLazy(new HashSet<Key> { new Key("Id", 1) });

        // Assert
        result.Should().Be(expected);
    }
}

internal class TestContext : DbContext
{
    public DbSet<TestModel> Models { get; set; }
    public DbSet<RelatedEntity> Relations { get; set; }
    public TestContext(DbContextOptions options): base(options)
    {

    }
}

internal class TestModel : IModel
{
    [Key]
    public int? Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RelatedEntity>? RelatedEntities { get; set; }

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class
    => values.Include("RelatedEntities");

    public void AutoIncrementPrimaryKey()
    => Id = null;

    public HashSet<Key> GetPrimaryKey()
    => new() { new(nameof(Id), Id) };

    public void SetPrimaryKey(HashSet<Key> primaryKey)
    => Id = (int?)primaryKey.Single(x => x.Name == nameof(Id)).Value;
}

internal class RelatedEntity : IModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public static IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class
    => values;

    public void AutoIncrementPrimaryKey()
    => Id = null;

    public HashSet<Key> GetPrimaryKey()
    => new() { new(nameof(Name), Id) };

    public void SetPrimaryKey(HashSet<Key> primaryKey)
    => Id = (int?)primaryKey.Single(x => x.Name == nameof(Id)).Value;
}