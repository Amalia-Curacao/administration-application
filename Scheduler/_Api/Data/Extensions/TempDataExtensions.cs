using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;
public static class TempDataExtensions
{
    private static readonly JsonSerializerOptions NonCycleJsonSerializationOptions = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };

    /// <summary> This method is used to store complex data in the TempData.</summary>
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
    {
        var data = JsonSerializer.Serialize(value, typeof(T), NonCycleJsonSerializationOptions);
        tempData[key] = data;
    }

    /// <summary> This method is used to retrieve complex data from TempData for one request. </summary>
    /// <param name="key"> The key to fetch the value. </param>
    public static T? Get<T>(this ITempDataDictionary tempData, string key)
    {
        object? o = null;
        tempData.TryGetValue(key, out o);
        if(o is null) return default;
        var result = JsonSerializer.Deserialize<T>((string)o, NonCycleJsonSerializationOptions);
        return result;
    }

    /// <summary> This method is used to retrieve complex data from TempData without removing it. </summary>
    public static T? Peek<T>(this ITempDataDictionary tempData, string key)
    {
        object? o = tempData.Peek(key);
        if(o is null) return default;
        var result = JsonSerializer.Deserialize<T>((string)o, NonCycleJsonSerializationOptions);
        return result;
    }

    /// <summary> This method is used to check if the TempData contains a value for the given key. </summary>
    public static bool IsNull(this ITempDataDictionary tempData, string key)
		=> tempData.Peek(key) is null;
}