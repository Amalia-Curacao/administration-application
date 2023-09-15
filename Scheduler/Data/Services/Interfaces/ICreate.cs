namespace Scheduler.Data.Services.Interfaces;


public interface ICreate<T>
{
	Task<bool> Add(T obj);
}
