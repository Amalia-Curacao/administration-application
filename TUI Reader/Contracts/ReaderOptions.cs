namespace TUI_Reader.Contracts;

/// <summary>
/// Options for the <see cref="Reader"/>.
/// </summary>
public class ReaderOptions
{
	/// <summary>
	/// Indicates if logging should be done for the reader.
	/// </summary>
	public bool Logging { get; init; } = true;
	/// <summary>
	/// Options for creating wed drivers.
	/// </summary>
	public DriverOptions DriverOptions { get; init; } = new();
	/// <summary>
	/// The maximum amount of parallel operations.
	/// </summary>
	public int MaximumParallelOperations { get; init; } = 5;
}