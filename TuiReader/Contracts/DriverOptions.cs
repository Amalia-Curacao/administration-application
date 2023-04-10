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
}