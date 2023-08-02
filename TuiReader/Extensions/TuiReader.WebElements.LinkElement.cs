namespace TuiReader.WebElements;

/// <summary>
/// Extensions for <see cref="LinkElement"/>.
/// </summary>
internal static class LinkElementExtensions
{
	/// <summary>
	/// Gathers all links on the page.
	/// </summary>
	/// <returns>All links on the page.</returns>
	public static IEnumerable<string> GetLinkReferences(this IEnumerable<LinkElement> linkElements) 
		=> linkElements.Select(element => element.Element.GetAttribute("href"));
}