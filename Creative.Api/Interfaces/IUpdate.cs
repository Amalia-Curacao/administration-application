namespace CreativeApi.Interfaces;

public interface IUpdate<T>
{
    /// <summary> Update object. </summary>
    /// <param name="toUpdate"> Object to update. </param>
    public void Update(params (int, T)[] toUpdate);
}
