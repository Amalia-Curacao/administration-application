namespace OpenQA.Selenium;

/// <summary>
/// Extensions for <see cref="IWebElement"/>.
/// </summary>
internal static class IWebElementExtensions
{
	
	/// <summary>
	/// Determines if <see cref="element"/> has a child or itself has the desired content.
	/// </summary>
	/// <param name="element">HTML element.</param>
	/// <param name="content">The content you want to find.</param>
	/// <returns>If a child or the element itself has the content</returns>
	public static bool HasContent(this IWebElement element, string content)
		=> (element.GetAttribute("innerText") 
			?? throw new Exception("No \"innerText found\" in element")).Contains(content) 
		   || element.GetChildren().Any(child => HasContent(child, content));
	
	
	/// <summary>
	/// Returns the element's immediate children.
	/// </summary>
	public static IEnumerable<IWebElement> GetChildren(this IWebElement element)
		=> element.FindElements(By.XPath("./*"));
	/// <summary>
	/// Get the innerText value from the given element.
	/// </summary>
	/// <param name="element">A HTML element.</param>
	/// <returns>InnerText value as a string</returns>
	/// <exception cref="Exception">No innerText value found in the <see cref="element"/></exception>
	public static string GetInnerText(this IWebElement element) 
		=> element.GetAttribute("innerText") ?? throw new Exception("No innerText value could be found in the element");

	
}