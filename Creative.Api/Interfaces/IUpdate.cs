namespace Creative.Api.Interfaces;

public interface IUpdate<T>
{
    /// <summary> Updates object in the database. </summary>
    /// <param name="obj"> Object to update. </param>
    /// <returns> The updated object. </returns>
    Task<T> Update(T obj);
}
