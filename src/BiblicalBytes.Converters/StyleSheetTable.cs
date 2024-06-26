using BiblicalBytes.Contracts;
using BiblicalBytes.Converters.RtfTree;

namespace BiblicalBytes.Converters;

/// <summary>
/// Represents a table of style sheets, allowing for addition and retrieval of style sheets by index.
/// </summary>
/// <param name="stylesheets">The initial dictionary of stylesheets, indexed by integer.</param>
/// <param name="logger">The logger instance for logging operations within the stylesheet table.</param>
public class StylesheetTable(IDictionary<int, RtfStyleSheet> stylesheets, ILogger logger) : IStyleTable<RtfStyleSheet>
{
    /// <summary>
    /// Adds a new stylesheet with an automatically assigned index to the stylesheet table.
    /// </summary>
    /// <param name="styleSheet">The stylesheet to add.</param>
    public void Add(RtfStyleSheet styleSheet)
    {
        stylesheets.Add(NewStylesheetIndex(), styleSheet);
    }

    /// <summary>
    /// Adds a stylesheet with a specified index to the stylesheet table.
    /// </summary>
    /// <param name="index">The index of the stylesheet to add.</param>
    /// <param name="styleSheet">The stylesheet to add.</param>
    public void Add(int index, RtfStyleSheet styleSheet)
    {
        stylesheets.Add(index, styleSheet);
    }

    /// <summary>
    /// Gets the stylesheet by its index.
    /// </summary>
    /// <param name="styleSheet">The stylesheet to find.</param>
    /// <returns>The index of the found stylesheet.</returns>
    public int IndexOf(RtfStyleSheet styleSheet)
    {
        return stylesheets.FirstOrDefault(x => x.Value.Equals(styleSheet)).Key;
    }

    /// <summary>
    /// Removes a stylesheet by index from the stylesheet table.
    /// </summary>
    /// <param name="index">The index of the stylesheet to remove.</param>
    public void Remove(int index)
    {
        stylesheets.Remove(index);
    }

    /// <summary>
    /// Removes a stylesheet from the stylesheet table.
    /// </summary>
    /// <param name="styleSheet">The stylesheet to remove.</param>
    public void Remove(RtfStyleSheet styleSheet)
    {
        stylesheets.Remove(styleSheet.Index);
    }

    /// <summary>
    /// Gets the stylesheet by its index.
    /// </summary>
    /// <param name="index">The index of the stylesheet.</param>
    /// <returns>The found stylesheet.</returns>
    public RtfStyleSheet this[int index] => stylesheets[index];

    /// <summary>
    /// Gets the number of stylesheets in the table.
    /// </summary>
    public int Count => stylesheets.Count;
    
    /// <summary>
    /// Finds the index of a stylesheet by its name.
    /// </summary>
    /// <param name="name">The name of the stylesheet to find.</param>
    /// <returns>The index of the found stylesheet.</returns>
    public int IndexOf(string name)
    {
        return stylesheets.FirstOrDefault(x => x.Value.Name.Equals(name)).Key;
    }

    /// <summary>
    /// Generates a new index for a stylesheet.
    /// </summary>
    /// <returns>A new stylesheet index is returned.</returns>
    private int NewStylesheetIndex()
    {
        return stylesheets.Count == 0 ? 0 : stylesheets.Keys.Max() + 1;
    }
}