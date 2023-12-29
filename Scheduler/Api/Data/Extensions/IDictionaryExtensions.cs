namespace System.Collections.Generic;

public static class IDictionaryExtensions
{
	public static bool ContentEquals<TKey, TValue>(this IDictionary<TKey, TValue> _dic, IDictionary<TKey, TValue> dictionary)
	where TKey : notnull
		=> _dic.Keys.Count == dictionary.Count
		&& _dic.Keys.All(dictionary.ContainsKey)
		&& _dic.All(kvp => dictionary[kvp.Key]!.Equals(kvp.Value));
}
