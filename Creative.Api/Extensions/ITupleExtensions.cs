namespace System.Runtime.CompilerServices;

/// <summary> Extensions for the ITuple interface. </summary>
public static partial class ITupleExtensions
{
    /// <summary> Get the value of a field in a tuple. </summary>
    public static T? Get<T>(this ITuple tuple, string name) => (T?)Get(tuple, name);

    /// <summary> Get the value of a field in a tuple. </summary>
    public static object? Get(this ITuple tuple, string name)
    {
        var fields = tuple.GetType().GetFields();
        foreach(var field in fields)
        {
            if(field.Name.Equals(name))
            {
                return field.GetValue(tuple);
            }
        }
        throw new Exception($"No field found with the name: \'{name}\'.");
    }

    /// <summary> Creates an <see cref="object[]"/> of tuple. </summary>
    public static object[] ToArray(this ITuple tuple) => tuple.ToEnumerable().ToArray();

    /// <summary> Creates an <see cref="IEnumerable{object}"/> of the tuple. </summary>
    public static IEnumerable<object> ToEnumerable(this ITuple tuple)
    {
        for (int i = 0; i < tuple.Length; i++)
        {
            yield return tuple[i]!;
        }
    }

    /// <summary> Checks if <paramref name="other"/> equals <paramref name="tuple"/>. </summary>
    public static bool Equals(this ITuple tuple, object? other) => other is ITuple otherTuple && tuple.Equals(otherTuple);


    // TODO: Field comparison not working
    /// <summary> Checks if <paramref name="otherTuple"/> equals <paramref name="tuple"/>. </summary>
    public static bool Equals(this ITuple tuple, ITuple otherTuple)
    {
        if (tuple.Length != otherTuple.Length) return false;
        if(tuple.Length == 1)
        {
            var tupleValue = tuple[0];
            var otherTupleValue = otherTuple[0];
            if (tupleValue is null && otherTupleValue is null) return true;
            return tupleValue.Equals(otherTupleValue);
        }
        foreach(var field in tuple.GetType().GetFields())
        {
            if(tuple.Get(field.Name) != otherTuple.Get(field.Name))
            {
                return false;
            }
        }
        return true;
    }
}
