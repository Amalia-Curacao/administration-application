using CreativeApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CreativeApi.Implementations.Entity_Framework;

/// <summary> Implementation of the Create Read Update Delete operations on a ef core database. </summary>
/// <typeparam name="T">Object in the database.</typeparam>
public class Crud<T> : ICreate<T>, IRead<T>, IUpdate<T>, IDelete where T : class 
{
    public Crud(DbContext dbContext) { DbContext = dbContext; }
    private DbContext DbContext { get; init; }

    /// <summary> Creates object(s) in database. </summary>
    public async void Create(params T[] toCreate)
    {
        await DbContext.Set<T>().AddRangeAsync(toCreate);
        await DbContext.SaveChangesAsync();
    }

    /// <summary> Gets all objects in the database. </summary>
    public T[]? Read() => DbContext.Set<T>().ToArray();

    /// <summary> Gets object from primary key. </summary>
    /// <param name="keys">Primary key(s) for object to get.</param>
    public T?[]? Read(params int[] keys) => keys.Select(key => DbContext.Find<T>(key)).ToArray();

    /// <summary> Update object(s) in database with new values. </summary>
    /// <param name="toUpdate"> Object(s) to update in the database. (Primary key, object with desired). </param>
    public async void Update(params (int, T)[] toUpdate)
    {
        foreach (var (key, newValues) in toUpdate)
        {
            var oldObject = Read(key)!.SingleOrDefault() ?? throw new Exception();
            DbContext.Entry(oldObject).CurrentValues.SetValues(newValues);
        }
        await DbContext.SaveChangesAsync();
    }

    /// <summary> Delete objects in database. </summary>
    /// <param name="keys">Primary keys of objects to delete. </param>
    /// <remarks> Will not delete any object if one of the keys cannot be found. </remarks>
    public async void Delete(params int[] keys)
    {
        var toDelete = Read(keys);
        if (toDelete is null) throw new Exception("No object found.");
        if (toDelete.Any(obj => obj is null)) throw new Exception("Contains key with no object.");
        else DbContext.Set<T>().RemoveRange(toDelete as T[]);
        await DbContext.SaveChangesAsync();
    }
}
