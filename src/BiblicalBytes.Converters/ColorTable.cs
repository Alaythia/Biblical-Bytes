using System.Drawing;
using BiblicalBytes.Contracts;

namespace BiblicalBytes.Converters;

/// <summary>
/// Represents a table of colors, allowing for addition and retrieval of colors by index.
/// </summary>
/// <param name="colors">The initial dictionary of colors, indexed by an integer.</param>
/// <param name="logger">The logger instance for logging operations within the color table.</param>
public class ColorTable(IDictionary<int, Color> colors, ILogger logger) : IStyleTable<Color>
{
    /// <summary>
    /// Adds a new color with an automatically assigned index to the color table.
    /// </summary>
    /// <param name="color">The color to add.</param>
    public void Add(Color color)
    {
        colors.Add(NewColorIndex(), color);
    }

    /// <summary>
    /// Adds a color with a specified index to the color table.
    /// </summary>
    /// <param name="index">The index at which to add the color.</param>
    /// <param name="color">The color to add.</param>
    public void Add(int index, Color color)
    {
        if (!colors.TryAdd(index, color))
            logger.Warning($"Color with index {index} already exists in the color table.");
    }

    /// <summary>
    /// Gets the color by its index.
    /// </summary>
    /// <param name="index">The index of the color to retrieve.</param>
    /// <returns>The color at the specified index.</returns>
    public Color this[int index] => colors[index];

    /// <summary>
    /// Gets the number of colors in the table.
    /// </summary>
    public int Count => colors.Count;

    /// <summary>
    /// Finds the index of a color.
    /// </summary>
    /// <param name="color">The color to find.</param>
    /// <returns>The index of the color if found; otherwise, the default key value indicating not found.</returns>
    public int IndexOf(Color color)
    {
        const int notFound = -1;

        try
        {
            return colors.Count == 0 
                ? notFound 
                : colors.First(x => x.Value.Equals(color)).Key;
        }
        catch (Exception e)
        {
            return notFound;
        }
    }

    /// <summary>
    /// Finds the index of a color by its name.
    /// </summary>
    /// <param name="name">The name of the color to find.</param>
    /// <returns>The index of the found color.</returns>
    public int IndexOf(string name)
    {
        const int notFound = -1;

        try
        {
            return colors.Count == 0
                ? notFound
                : colors.First(x => x.Value.Name.Equals(name)).Key;
        }
        catch (Exception e)
        {
            return notFound;
        }
    }

    /// <summary>
    /// Removes a color by index from the color table.
    /// </summary>
    /// <param name="index">The index of the color to remove.</param>
    public void Remove(int index)
    {
        colors.Remove(index);
    }

    /// <summary>
    /// Removes a color from the color table.
    /// </summary>
    /// <param name="color">The color to remove.</param>
    public void Remove(Color color)
    {
        var index = IndexOf(color);
        if (index != -1)
            colors.Remove(index);
    }

    /// <summary>
    /// Generates a new color index that is not currently used in the color table.
    /// </summary>
    /// <returns>A new, unique color index.</returns>
    private int NewColorIndex()
    {
        return colors.Count == 0 ? 0 : colors.Keys.Max() + 1;
    }
}
