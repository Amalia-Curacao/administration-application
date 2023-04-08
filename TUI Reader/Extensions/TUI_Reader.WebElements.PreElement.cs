namespace TUI_Reader.WebElements;

internal static class PreElementExtensions
{
	
	/// <summary>
	/// Gets a text content from <see cref="PreElement">element</see>.
	/// </summary>
	public static string GetContent(this PreElement preElement)
		=> preElement.Element.GetAttribute("textContent") ?? throw new Exception("No text content could be found in the element");
}