using TuiReader.Contracts;

Console.WriteLine("-----Start TuiReader Console-----");
Console.WriteLine();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();
var readerOptions = new ReaderContext
{
	Logging = true,
	MaximumParallelOperations = 5,
	DriverOptions = new DriverOptions
	{
		Logging = false,
		Headless = /*Environment.GetCommandLineArgs().Any(arg => arg.Equals("--headless"))*/ true
	}
};
await readerOptions.Execute();
watch.Stop();

Console.WriteLine($"Execution time: {watch.Elapsed.Minutes} minutes");
Console.WriteLine();
Console.WriteLine("-----End TuiReader Console-----");