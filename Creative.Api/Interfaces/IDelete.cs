using System.Runtime.CompilerServices;

namespace Creative.Api.Interfaces;

public interface IDelete
{
    /// <summary> Delete object(s) with id. </summary>
    /// <param name="id"> Object's primary key(s) to delete. </param>
    void Delete(IDictionary<string, object> id);
}
