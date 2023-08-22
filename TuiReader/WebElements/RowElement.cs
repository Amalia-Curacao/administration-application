using OpenQA.Selenium;
using TUI_Reader.Extensions;

namespace TUI_Reader.WebElements;
/// <summary>
/// Facade for <see cref="IWebElement"/>.
/// </summary>
internal class RowElement
{
    /// <summary>
    /// <inheritdoc cref="IWebElement"/>
    /// </summary>
    public IWebElement Element { get; init; } = null!;


    /// <summary>
    /// Gets the child in the row element that contains the values.
    /// </summary>
    public IWebElement GetValue(string name)
    {
        foreach (var child in Element.GetChildren())
        {
            var childValue = child.GetAttribute("innerText") ?? throw new Exception("No innerText was found in child element.");
            if (!childValue.Contains(name))
                return child;
        }
        throw new Exception("No \"value child\" was found");
    }
}