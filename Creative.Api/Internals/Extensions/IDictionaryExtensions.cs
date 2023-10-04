namespace System.Collections.Generic;
internal static class IDictionaryExtensions
{
	public static bool Equals(this IDictionary<string, object> id, IDictionary<string, object> other)
	{
		if (id.Count != other.Count) return false;
		foreach (var key in id.Keys)
		{
			if (!other.ContainsKey(key)) return false;
			if (!id[key].Equals(other[key])) return false;
		}
		return true;
	}
}
