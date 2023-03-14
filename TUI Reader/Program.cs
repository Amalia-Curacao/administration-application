using System.Text.Json;
using TUI_Reader.Properties;

Console.WriteLine("-----Start TUI Reader Console-----");


var appSettings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(@"./Properties/appsettings.json"));


Console.WriteLine("-----End TUI Reader Console-----");