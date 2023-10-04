namespace Creative.Api.Interfaces;

public interface ICreate<in T>
{
    /// <summary> Adds object to the database. </summary>
    Task<bool> Add(T obj);
}