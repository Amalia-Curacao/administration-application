using Creative.Test.Utils;
using CreativeApi.Implementations.Entity_Framework;
using FluentAssertions;

namespace Creative.Test.Api._Crud;

public class _Crud
{

    [TestFixture]
    public class Create : MockDatabase
    {
        [Test]
        public void a_singular_object_in_database()
        {
            var newObject = new SimpleObject { PrimaryKey = new Key(7) , Value = "value" };
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Create(newObject);

            _dbContext.Objects.Should().Contain(newObject);
        }

        [Test]
        public void multiple_objects_in_single_database()
        {
            var newObjects = new[]
            {
                new SimpleObject{ PrimaryKey = new Key(7) , Value="value" },
                new SimpleObject{ PrimaryKey = new Key(8) , Value="value" },
            };
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Create(newObjects);

            _dbContext.Objects.Should().Contain(newObjects);
        }
    }

    [TestFixture]
    public class Read : MockDatabase
    {
        [Test]
        public void get_all_objects()
        {
            var crud = new Crud<SimpleObject>(_dbContext);
            
            crud.Read().Should().BeEquivalentTo(seed_database());
        }

        [Test]
        public void get_single_object_from_database_with_primary_key()
        {
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Read(1).Should().BeEquivalentTo(new[] { seed_database()[0] });
        }
            
        [Test]
        public void get_multiple_object_from_database_with_primary_key()
        {
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Read(1, 2).Should().BeEquivalentTo(new[] { seed_database()[0], seed_database()[1] });
        }
    }

    [TestFixture]
    public class Update : MockDatabase
    {
        [Test]
        public void single_object_value() 
        {
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Update((1, new SimpleObject { PrimaryKey = 1, Value = "other value"}));

            _dbContext.Objects.Single(obj => obj.PrimaryKey == 1).Value.Should().Be("other value");
        }

        [Test]
        public void multiple_objects_value()
        {
            var crud = new Crud<SimpleObject>(_dbContext);

            crud.Update((1, new SimpleObject { PrimaryKey = 1, Value = "other value" }), (2, new SimpleObject { PrimaryKey = 2, Value = "other value" }));

            _dbContext.Objects.Single(obj => obj.PrimaryKey == 1).Value.Should().Be("other value");
            _dbContext.Objects.Single(obj => obj.PrimaryKey == 2).Value.Should().Be("other value");
        }
    }

    [TestFixture]
    public class Delete : MockDatabase
    {
        [Test]
        public void single_object() 
        {
            var crud = new Crud<SimpleObject>(_dbContext);
            var deleted = _dbContext.Objects.Single(obj => obj.PrimaryKey == 1);

            crud.Delete(1);

            _dbContext.Objects.Should().NotContain(deleted);
        }

        [Test]
        public void multiple_objects() 
        {
            var crud = new Crud<SimpleObject>(_dbContext);
            var deleted = new List<SimpleObject>() { _dbContext.Objects.Single(obj => obj.PrimaryKey == 1), _dbContext.Objects.Single(obj => obj.PrimaryKey == 2) };

            crud.Delete(1, 2);

            _dbContext.Objects.Should().NotContain(deleted);
        }
    }
}



