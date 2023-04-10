using TuiReader.Contracts;

Console.WriteLine("-----Start TuiReader Console-----");

var watch = new System.Diagnostics.Stopwatch();
watch.Start();
var readerOptions = new ReaderContext
{
	Logging = true,
	MaximumParallelOperations = 1,
	DriverOptions = new DriverOptions
	{
		Logging = false,
		Headless = Environment.GetCommandLineArgs().Any(arg => arg.Equals("--headless"))
	}
};
var controller = await readerOptions.Execute();
/*foreach (var notification in controller.ReadAll())
{
	Console.WriteLine(notification);
}*/
watch.Stop();

Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds}ms");

Console.WriteLine("-----End TuiReader Console-----");