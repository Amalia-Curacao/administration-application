using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

public static class TempDataExtensions
{
    /// <summary> This method is used to store complex data in the TempData.</summary>
    public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        var data = JsonSerializer.Serialize(value, typeof(T));
        tempData[key] = data;
    }

    /// <summary> This method is used to retrieve complex data from TempData for one request. </summary>
    /// <param name="key"> The key to fetch the value. </param>
    public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        object? o = null;
        tempData.TryGetValue(key, out o);
        if(o is null) return null;
        var result = JsonSerializer.Deserialize<T>((string)o);
        return result;
    }

    /// <summary> This method is used to retrieve complex data from TempData without removing it. </summary>
    public static T? Peek<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        object? o = tempData.Peek(key);
        if(o is null) return null;
        var result = JsonSerializer.Deserialize<T>((string)o);
        return result;
    }
}