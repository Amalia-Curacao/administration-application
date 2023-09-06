using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services.Interfaces;

public interface ICrud<T>
{   
    Task<IEnumerable<T>> GetAll();
    Task<T> Get(ITuple id);
    Task<T> GetLazy(ITuple id);
    void Add(T obj);
    Task<T> Update(T obj);
    void Delete(ITuple id);
}
