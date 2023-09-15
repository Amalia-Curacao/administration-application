using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services.Interfaces;

public interface IRead<T>
{
	Task<IEnumerable<T>> GetAll();
	IAsyncEnumerable<T> GetAllNoCycle();
	Task<T> Get(ITuple id);
	Task<T> GetLazy(ITuple id);
	Task<T> GetNoCycle(ITuple id);
}
