namespace Creative.Api.Interfaces;

public interface IDelete<T>
{
    /// <summary> Delete object(s) with id. </summary>
    /// <param name="id"> Object's primary key(s) to delete. </param>
    Task Delete(T obj);
}
