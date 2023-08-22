namespace CreativeApi.Interfaces;

public interface IDelete
{
    /// <summary> Delete object(s) with id. </summary>
    /// <param name="key"> Object's primary key(s) to delete. </param>
    public void Delete(params int[] keys);
}
