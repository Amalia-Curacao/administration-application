namespace CreativeApi.Interfaces;

public interface IRead<T>
{
    /// <summary> Gets all objects. </summary>
    public T[]? Read();

    /// <summary> Gets specific object(s) with its primary key(s). </summary>
    public T?[]? Read(params int[] keys);
}
