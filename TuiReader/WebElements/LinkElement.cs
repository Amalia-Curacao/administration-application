using OpenQA.Selenium;

namespace TuiReader.WebElements;

internal class LinkElement
{
	/// <summary>
	/// <inheritdoc cref="IWebElement"/>
	/// </summary>
	public IWebElement Element { get; init; } = null!;
}