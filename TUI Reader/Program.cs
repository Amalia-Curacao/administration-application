using TUI_Reader;
using TUI_Reader.Contracts;

Console.WriteLine("-----Start TUI Reader Console-----");

var options = new ReaderOptions
{
	Logging = true,
	DriverOptions = new DriverOptions
	{
		Logging = false,
		Headless = true
	}
};
var reader = new Reader
{
	ReaderOptions = options
};
foreach (var notification in await reader.GetNotifications())
{
	Console.WriteLine(notification);
}

Console.WriteLine("-----End TUI Reader Console-----");