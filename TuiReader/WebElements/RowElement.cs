using OpenQA.Selenium;

namespace TuiReader.WebElements;
/// <summary>
/// Facade for <see cref="IWebElement"/>.
/// </summary>
internal class RowElement
{
	/// <summary>
	/// <inheritdoc cref="IWebElement"/>
	/// </summary>
	public IWebElement Element { get; init; } = null!;
}