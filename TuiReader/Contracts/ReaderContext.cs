namespace TuiReader.Contracts;

/// <summary>
/// Options for the <see cref="Reader"/>.
/// </summary>
public class ReaderContext
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
	/// <summary>
	/// <inheritdoc cref="LoginContext"/>
	/// </summary>
	/// <remarks>
	/// If left empty will use value from the local appsettings.json.
	/// </remarks>
	public LoginContext LoginContext { get; init; } = LoginContext.DefaultLoginContext();


}