using Creative.Api.Interfaces;
using Creative.Api.Internal.Json;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Creative.Api.Implementations.Entity_Framework;

/// <summary> Implementation of the Create Read Update Delete operations on a ef core database. </summary>
/// <typeparam name="T">Object in the database.</typeparam>
public class Crud<T> : ICrud<T> where T : class, IModel 
{
    private static readonly JsonSerializerOptions NonCycleJsonSerializationOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
    private static readonly JsonSerializerOptions NoRelationJsonSerializationOptions = new() { ReferenceHandler = new NoReferenceHandler() };
    public Crud(DbContext dbContext) { DbContext = dbContext; }
    private DbContext DbContext { get; init; }

    public async Task<bool> Add(T obj)
    {
        obj.AutoIncrementPrimaryKey();
        await DbContext.Set<T>().AddAsync(obj);
        await DbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<T>> GetAll() => await T.IncludeAll(DbContext.Set<T>()).ToListAsync();

    public async IAsyncEnumerable<T> GetAllNoCycle()
    {
        foreach(var obj in await GetAll())
        {
            var serialized = JsonSerializer.Serialize(obj, NonCycleJsonSerializationOptions);
            yield return JsonSerializer.Deserialize<T>(serialized, NonCycleJsonSerializationOptions)!;
        }
    }

    public async Task<T> Get(ITuple id) => await DbContext.Set<T>().FindAsync(id.ToArray()) ?? throw new Exception("No object found.");

    public async Task<T> GetLazy(ITuple id)
    {
        var obj = await DbContext.Set<T>().FindAsync(id.ToArray());
        var json = JsonSerializer.Serialize(obj, NoRelationJsonSerializationOptions);
        return JsonSerializer.Deserialize<T>(json, NoRelationJsonSerializationOptions)!;
    }

    public async Task<T> GetNoCycle(ITuple id)
    {
        var obj = await DbContext.Set<T>().FindAsync(id.ToArray());
        var json = JsonSerializer.Serialize(obj, NonCycleJsonSerializationOptions);
        return JsonSerializer.Deserialize<T>(json, NonCycleJsonSerializationOptions)!;
    }

    public async Task<T> Update(T obj)
    {
        UpdateProperties(obj);
        await DbContext.SaveChangesAsync();
        return obj;
    }

    /// <summary> Updates the properties of the object in the database with the new values. </summary>
    private void UpdateProperties(IModel obj)
    {
        var oldObject = Get(obj.GetPrimaryKey())!;
        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);
            if (value is null) continue;
            DbContext.Entry(oldObject).Property(property.Name).CurrentValue = value;
        }
    }

    public async void Delete(ITuple id)
    {
        var obj = await Get(id) ?? throw new Exception("No object found.");
        DbContext.Set<T>().Remove(obj);
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
