namespace TuiReader;

/// <summary>
/// Extensions for <see cref="Driver"/>.
/// </summary>
internal static class DriverExtensions
{
    
	/// <summary>
	/// Logs multiple <see cref="Driver"/> in.
	/// </summary>
	/// <returns>Logged in drivers.</returns>
	public static IEnumerable<Driver> Login(this IEnumerable<Driver> drivers, LoginContext context)
		=> drivers.Select(driver => driver.Login(context));

	/// <summary>
	/// Dispose all <see cref="Driver">drivers</see>.
	/// </summary>
	public static void Dispose(this IEnumerable<Driver> drivers)
	{
		foreach (var driver in drivers)
		{
			driver.Dispose();
		}
	}
}