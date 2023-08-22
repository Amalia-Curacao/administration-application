namespace Creative.Database.Interfaces;

public interface IKey<DbType> : IComparable
{
    public DbType?[]? DbKeyCollection();

}
