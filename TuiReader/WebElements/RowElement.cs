using OpenQA.Selenium;

namespace TuiReader.WebElements;

internal class RowElement
{
	/// <summary>
	/// <inheritdoc cref="IWebElement"/>
	/// </summary>
	public IWebElement Element { get; init; } = null!;
}