namespace Creative.Api.Interfaces;

public interface IRead<T>
{
    /// <summary> Gets all <see cref="T"/> entities from the database. </summary>
    Task<IEnumerable<T>> GetAll();

	/// <summary> Gets all <see cref="T"/> entities from the database, without relationships that cycle. </summary>
	Task<IEnumerable<T>> GetAllNoCycle();

    /// <summary> Gets specific <see cref="T"/> entity based on primary key. </summary>
    Task<T> Get(IDictionary<string, object> id);

    /// <summary> Gets specific <see cref="T"/> entity based on primary key, without relationships. </summary>
    Task<T> GetLazy(IDictionary<string, object> id);

    /// <summary> Gets specific <see cref="T"/> entity based on primary key, without relationships that cycle. </summary>
    Task<T> GetNoCycle(IDictionary<string, object> id);
}
