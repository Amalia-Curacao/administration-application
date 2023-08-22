using OpenQA.Selenium;

namespace TUI_Reader.WebElements;
/// <summary>
/// Facade for <see cref="IWebElement"/>.
/// </summary>
internal class PreElement
{
    /// <summary>
    /// <inheritdoc cref="IWebElement"/>
    /// </summary>
    public IWebElement Element { get; init; } = null!;


    /// <summary>
    /// Gets a text content from <see cref="PreElement">element</see>.
    /// </summary>
    public string GetContent()
        => Element.GetAttribute("textContent") ?? throw new Exception("No text content could be found in the element");
}