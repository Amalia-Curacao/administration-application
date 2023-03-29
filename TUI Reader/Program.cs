using TUI_Reader;
using TUI_Reader.Contracts;

Console.WriteLine("-----Start TUI Reader Console-----");

var options = new ReaderOptions
{
	Logging = true,
	DriverOptions = new DriverOptions
	{
		Logging = false,
		Headless = false
	},
	MaximumParallelOperations = 5
};
var reader = new Reader(options);
foreach (var notification in await reader.GetNotifications())
{
	Console.WriteLine(notification);
}

Console.WriteLine("-----End TUI Reader Console-----");