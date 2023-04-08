using OpenQA.Selenium;

namespace TUI_Reader.WebElements;

internal class RowElement
{
	/// <summary>
	/// <inheritdoc cref="IWebElement"/>
	/// </summary>
	public IWebElement Element { get; init; } = null!;
}