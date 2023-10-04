using Creative.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Creative.Api.Implementations.Entity_Framework;

/// <summary> Implementation of the Create Read Update Delete operations on a ef core database. </summary>
/// <typeparam name="T">Object in the database.</typeparam>
public class Crud<T> : ICrud<T> where T : class, IModel 
{
    private static readonly JsonSerializerOptions NonCycleJsonSerializationOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
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

    public async Task<T> Get(IDictionary<string,object> id)
    {
        var all = await GetAll();
        var obj = all.FirstOrDefault(e => IDictionaryExtensions.Equals(e.GetPrimaryKey(), id));
        return obj ?? throw new Exception("No object found.");
    } 

    public async Task<T> GetLazy(IDictionary<string, object> id) 
        => await DbContext.Set<T>().FindAsync(id.Select(i => i.Value).ToArray()) ?? throw new Exception("No object found.");

    public async Task<T> GetNoCycle(IDictionary<string, object> id)
    {
        var obj = await Get(id);
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
    private async void UpdateProperties(IModel obj)
    {
        var oldObject = await Get(obj.GetPrimaryKey())!;
        foreach (var property in obj.GetType().GetProperties())
        {
            if (!property.PropertyType.IsValueType) continue;
            var value = property.GetValue(obj);
            DbContext.Entry(oldObject).Property(property.Name).CurrentValue = value;
        }
    }

    public async void Delete(IDictionary<string, object> id)
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
}
