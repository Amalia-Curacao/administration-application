using OpenQA.Selenium;

namespace TUI_Reader.WebElements;
/// <summary>
/// Facade for <see cref="IWebElement"/>.
/// </summary>
internal class TdElement
{

    /// <summary>
    /// <inheritdoc cref="IWebElement"/>
    /// </summary>
    public IWebElement Element { get; init; } = null!;
}