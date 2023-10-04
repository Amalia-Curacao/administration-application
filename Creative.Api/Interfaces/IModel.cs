﻿using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Creative.Api.Interfaces;

public interface IModel
{
    /// <summary> Get the primary key of the object. </summary>
    public IDictionary<string, object> GetPrimaryKey();

    /// <summary> Set the primary key of the object. </summary>
    /// <remarks> Use tuple field names corresponding with primary key(s) name.</remarks>
    public void SetPrimaryKey(IDictionary<string, object> primaryKey);

    /// <summary> Set auto-increment primary-key to null. </summary>
    /// <remarks> Set all auto-increment primary-keys to null here. </remarks>
    public void AutoIncrementPrimaryKey();

    /// <summary> Includes all object relations in query even multiple layers deep. </summary>
    public static abstract IQueryable<T> IncludeAll<T>(DbSet<T> values) where T : class;
}