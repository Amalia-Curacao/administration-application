namespace Scheduler.Data.Services.Interfaces;

public interface ICrud<T> : ICreate<T>, IRead<T>, IUpdate<T>, IDelete<T> {   }