using System.Runtime.CompilerServices;

namespace Scheduler.Data.Services.Interfaces;

public interface IDelete<T>
{
	void Delete(ITuple id);
}
