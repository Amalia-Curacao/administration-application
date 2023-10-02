using System.Runtime.CompilerServices;

namespace Creative.Api.Interfaces;

public interface IRead<T>
{
    /// <summary> Gets all <see cref="T"/> entities from the database. </summary>
    Task<IEnumerable<T>> GetAll();

    /// <summary> Gets all <see cref="T"/> entities from the database, without relationships that cycle. </summary>
    IAsyncEnumerable<T> GetAllNoCycle();

    /// <summary> Gets specific <see cref="T"/> entity based on primary key. </summary>
    Task<T> Get(ITuple id);

    /// <summary> Gets specific <see cref="T"/> entity based on primary key, without relationships. </summary>
    Task<T> GetLazy(ITuple id);

    /// <summary> Gets specific <see cref="T"/> entity based on primary key, without relationships that cycle. </summary>
    Task<T> GetNoCycle(ITuple id);
}
