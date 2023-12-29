namespace System.Collections.Generic;

internal static class IAsyncIEnumerableExtensions
{
	/// <summary> Awaits <see cref="IAsyncEnumerable{T}"/> to a list. </summary>
	public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
	{
		var list = new List<T>();
		await foreach (var item in source)
		{
			list.Add(item);
		}
		return list;
	}
}
