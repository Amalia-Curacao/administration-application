namespace TuiReader.Contracts;

/// <summary>
/// Options for web drivers.
/// </summary>
public class DriverOptions
{
	/// <summary>
	/// Indicates if logging should be written.
	/// </summary>
	public bool Logging { get; init; } = false;
	/// <summary>
	/// Indicates if browsers will be headless.
	/// </summary>
	public bool Headless { get; init; } = true;
	/// <summary>
	/// Creates <see cref="Driver">drivers</see>.
	/// </summary>
	internal IEnumerable<Driver> Create(int amount = 1)
	{
		for (var i = 0; i < amount; i++)
		{
			yield return Driver.Chrome(this);
		}
	}
}