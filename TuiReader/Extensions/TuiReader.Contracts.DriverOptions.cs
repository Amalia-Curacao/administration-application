namespace TuiReader.Contracts;

/// <summary>
/// Extensions for <see cref="DriverOptions"/>
/// </summary>
internal static class DriverOptionsExtensions
{
	/// <summary>
	/// Creates <see cref="Driver">drivers</see>.
	/// </summary>
	public static IEnumerable<Driver> Create(this DriverOptions options, int amount = 1)
	{
		for (var i = 0; i < amount; i++)
		{
			yield return Driver.Chrome(options);
		}
	}
}