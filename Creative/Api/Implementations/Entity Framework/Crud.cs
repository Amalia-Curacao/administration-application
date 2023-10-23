using Creative.Api.Data;
using Creative.Api.Exceptions;
using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>Implementation of the Create Read Update Delete operations on a ef core database. </summary>
/// <typeparam name="T">Object in the database.</typeparam>
public class Crud<T> : ICrud<T> where T : class, IModel 
{
    /// <summary> The DbContext used for database operations. </summary>
    private DbContext DbContext { get; init; }

    /// <summary> Initializes a new instance of the <see cref="Crud{T}"/> class. </summary>
    /// <param name="dbContext">The DbContext used for database operations.</param>
    public Crud(DbContext dbContext) { DbContext = dbContext; }

    /// <summary> Adds one or more objects to the database. </summary>
    /// <param name="AutoIncrementPrimaryKey">Whether to auto-increment the primary key of the added objects.</param>
    /// <param name="objs">The objects to add to the database.</param>
    /// <returns>The added objects.</returns>
    public async Task<T[]> Add(bool AutoIncrementPrimaryKey = true, params T[] objs){
        foreach(var obj in objs){
            if(AutoIncrementPrimaryKey) obj.AutoIncrementPrimaryKey();
            await DbContext.Set<T>().AddAsync(obj);
        }
        await DbContext.SaveChangesAsync();
        return objs!;
    }

    /// <summary> Attempts to add one or more objects to the database. </summary>
    /// <param name="AutoIncrementPrimaryKey">Whether to auto-increment the primary key of the added objects.</param>
    /// <param name="objs">The objects to add to the database.</param>
    /// <returns>The added objects, or null if an exception occurred.</returns>
    public async Task<T[]?> TryAdd(bool AutoIncrementPrimaryKey = true, params T[] objs)
    {
        try
        {
            return await Add(AutoIncrementPrimaryKey, objs);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary> Gets all objects from the database. </summary>
    /// <returns>All objects from the database.</returns>
    public async Task<T[]> GetAll() 
    => await T.IncludeAll(DbContext.Set<T>()).ToArrayAsync();

    /// <summary> Gets an object from the database by its primary key. </summary>
    /// <param name="key">The primary key of the object to get.</param>
    /// <returns>The object with the specified primary key, or null if it does not exist.</returns>
    public T? TryGet(HashSet<Key> key)
    => TryGet(new[] { key })?.Single();

    /// <summary> Gets an object from the database by its primary key. </summary>
    /// <param name="key">The primary key of the object to get.</param>
    /// <returns>The object with the specified primary key.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown if the object with the specified primary key does not exist.</exception>
    public T Get(HashSet<Key> key)
    => Get(new[] { key }).Single();

    /// <summary> Gets objects from the database by their primary keys. </summary>
    /// <param name="keys">The primary keys of the objects to get.</param>
    /// <returns>The objects with the specified primary keys, or null if one does not exist.</returns>
    public T[]? TryGet(params HashSet<Key>[] keys)
    {
        try
        {
            return Get(keys);
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
    }

    /// <summary> Gets objects from the database by their primary keys. </summary>
    /// <param name="keys">The primary keys of the objects to get.</param>
    /// <returns>The objects with the specified primary keys.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown if an object with one of the specified primary keys does not exist.</exception>
    public T[] Get(params HashSet<Key>[] keys)
    {
        var list = new List<T>();
        foreach (var key in keys)
        {
            T? obj = null;
            foreach(var item in T.IncludeAll(DbContext.Set<T>()))
            {
                if(item.GetPrimaryKey().SetEquals(key))
                    obj = item;
            }
			list.Add(obj ?? throw new ObjectNotFoundException());
		}
        return list.ToArray();
    }

    /// <summary> Gets all objects from the database without loading their related entities. </summary>
    /// <returns>All objects from the database.</returns>
    public T[] GetAllLazy()
    => DbContext.Set<T>().ToArray();

    /// <summary> Gets objects from the database by their primary keys without loading their related entities. </summary>
    /// <param name="keys">The primary keys of the objects to get.</param>
    /// <returns>The objects with the specified primary keys.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown if an object with one of the specified primary keys does not exist.</exception>
    public T[] GetLazy(params HashSet<Key>[] keys)
    {
        var list = new List<T>();
        foreach(var key in keys)
        {
            T? obj = null;
            foreach(var item in DbContext.Set<T>())
            {
                if (item.GetPrimaryKey().SetEquals(key)) obj = item;
            }
            list.Add(obj ?? throw new ObjectNotFoundException());
        }
        return list.ToArray();
    }

    /// <summary> Gets an object from the database by its primary key without loading its related entities. </summary>
    /// <param name="key">The primary key of the object to get.</param>
    /// <returns>The object with the specified primary key.</returns>
    /// <exception cref="ObjectNotFoundException">Thrown if the object with the specified primary key does not exist.</exception>
    public T GetLazy(HashSet<Key> key)
    => GetLazy(new[] { key }).Single();

    /// <summary> Gets objects from the database by their primary keys without loading their related entities. </summary>
    /// <param name="keys">The primary keys of the objects to get.</param>
    /// <returns>The objects with the specified primary keys, or null if one does not exist.</returns>
    public T[]? TryGetLazy(params HashSet<Key>[] keys)
    {
        try
        {
            return GetLazy(keys);
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
    }

    /// <summary> Gets an object from the database by its primary key without loading its related entities. </summary>
    /// <param name="key">The primary key of the object to get.</param>
    /// <returns>The object with the specified primary key, or null if it does not exist.</returns>
    public T? TryGetLazy(HashSet<Key> key)
    => TryGetLazy(new[] { key })?.Single();

    /// <summary> Updates an object in the database. </summary>
    /// <param name="obj">The object to update.</param>
    /// <returns>The updated object.</returns>
    public async Task<T> Update(T obj)
    {
        // Updates the objects properties.
        var oldObject = Get(obj.GetPrimaryKey())!;
		foreach (var property in obj.GetType().GetProperties())
		{
			if (!property.PropertyType.IsValueType) continue;
			var value = property.GetValue(obj);
			DbContext.Entry(oldObject).Property(property.Name).CurrentValue = value;
		}
        await DbContext.SaveChangesAsync();
        return Get(obj.GetPrimaryKey());
    }

    /// <summary> Deletes objects from the database by their primary keys. </summary>
    /// <param name="keys">The primary keys of the objects to delete.</param>
    /// <returns>True if the objects were deleted successfully, false otherwise.</returns>
    /// <summary>
    /// Deletes the entities with the specified keys from the database.
    /// </summary>
    /// <param name="keys">The keys of the entities to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    public async Task<bool> Delete(params HashSet<Key>[] keys)
    {
        var objs = TryGet(keys);

        if(objs == null) 
        {
            return false;
        }

        DbContext.Set<T>().RemoveRange(objs);
        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}
