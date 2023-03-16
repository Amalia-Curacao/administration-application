using TUI_Reader;

Console.WriteLine("-----Start TUI Reader Console-----");

var notifications = new Reader().Run();
foreach (var notification in notifications)
{
    Console.WriteLine(notification.ToString());
}


Console.WriteLine("-----End TUI Reader Console-----");