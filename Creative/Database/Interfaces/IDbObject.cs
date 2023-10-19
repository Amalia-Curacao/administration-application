namespace Creative.Database.Interfaces;

public interface IDbObject<DbType>
{
    public IKey<DbType> PrimaryKey { get; init; }
}
