﻿namespace Creative.Api.Interfaces;

public interface ICrud<T> : IRead<T>, ICreate<T>, IUpdate<T>, IDelete where T : class
{
}
