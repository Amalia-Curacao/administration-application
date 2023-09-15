namespace Scheduler.Data.Services.Interfaces;

public interface IUpdate<T>
{
	Task<T> Update(T obj);
}
