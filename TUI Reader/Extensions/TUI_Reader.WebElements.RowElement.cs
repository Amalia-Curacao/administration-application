using OpenQA.Selenium;

namespace TUI_Reader.WebElements;

internal static class RowElementExtensions
{
	
	/// <summary>
	/// Gets the child in the row element that contains the values.
	/// </summary>
	public static IWebElement GetValue(this RowElement row, string name)
	{
		foreach (var child in row.Element.GetChildren())
		{
			var childValue = child.GetAttribute("innerText") ?? throw new Exception("No innerText was found in child element.");
			if(!childValue.Contains(name))
				return child;
		}
		throw new Exception("No \"value child\" was found");
	}
	/// <summary>
	/// Finds a row with a child that has the same content as <see cref="rowName"/>.
	/// </summary>
	public static RowElement FindRow(this IEnumerable<RowElement> rows, string rowName)
	{
		var foundOneRow = false;
		RowElement? returnRow = null;
		foreach (var row in rows)
		{
			if (!row.Element.HasContent(rowName)) continue;
            
			if (foundOneRow)
				throw new Exception($"Multiple rows found with the {rowName} as a row name.");
            
			returnRow = row;
			foundOneRow = true;
		}
		return returnRow ?? throw new Exception("Desired row could not be found.");
	}
	/// <summary>
	/// Gets a row value.
	/// </summary>
	public static string GetValue(this IEnumerable<RowElement> rows, string name)
		=> rows.FindRow(name).GetValue(name).GetInnerText();

}