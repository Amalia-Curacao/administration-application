namespace CreativeApi.Interfaces;

public interface ICreate<in T>
{
    /// <summary> Creates object. </summary>
    /// <param name="toCreate"> Object to create. </param>
    public void Create(params T[] toCreate);
}