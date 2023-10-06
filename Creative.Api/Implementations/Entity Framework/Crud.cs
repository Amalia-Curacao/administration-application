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

    public async Task<IEnumerable<T>> GetAll() 
        => await T.IncludeAll(DbContext.Set<T>()).ToListAsync();

    public async Task<IEnumerable<T>> GetAllNoCycle()
        => (await GetAll()).Select(obj => 
        JsonSerializer.Deserialize<T>(
            JsonSerializer.Serialize(obj, NonCycleJsonSerializationOptions), 
            NonCycleJsonSerializationOptions)!);
    
    public async Task<T> Get(IDictionary<string,object> id)
        => (await GetAll()).FirstOrDefault(e => IDictionaryExtensions.Equals(e.GetPrimaryKey(), id)) 
        ?? throw new Exception("No object found.");
    

    public async Task<T> GetLazy(IDictionary<string, object> id) 
        => await DbContext.Set<T>().FindAsync(id.Select(i => i.Value).ToArray()) 
        ?? throw new Exception("No object found.");

	public async Task<T> GetNoCycle(IDictionary<string, object> id)
		=> JsonSerializer.Deserialize<T>(
			JsonSerializer.Serialize(await Get(id), NonCycleJsonSerializationOptions),
			NonCycleJsonSerializationOptions)!;

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
			if (!property.PropertyType.IsValueType) continue;
			var value = property.GetValue(obj);
			DbContext.Entry(oldObject).Property(property.Name).CurrentValue = value;
		}
	}

	public async Task Delete(T obj)
    {
        
        DbContext.Set<T>().Remove(obj);
        await DbContext.SaveChangesAsync();
    }
}
