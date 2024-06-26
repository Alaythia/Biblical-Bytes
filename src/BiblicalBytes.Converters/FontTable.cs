using BiblicalBytes.Contracts;

namespace BiblicalBytes.Converters;

/// <summary>
/// Represents a table of fonts, allowing for addition and retrieval of fonts by index.
/// </summary>
/// <param name="fonts">The initial dictionary of fonts, indexed by an integer.</param>
/// <param name="logger">The logger instance for logging operations within the font table.</param>
public class FontTable(IDictionary<int, string> fonts, ILogger logger) : IStyleTable<string>
{
    /// <summary>
    /// Adds a new font with an automatically assigned index to the font table.
    /// </summary>
    /// <param name="name">The name of the font to add.</param>
    public void Add(string name)
    {
        fonts.Add(NewFontIndex(), name);
    }

    /// <summary>
    /// Adds a font with a specified index to the font table.
    /// </summary>
    /// <param name="index">The index at which to add the font.</param>
    /// <param name="name">The name of the font to add.</param>
    /// <remarks>
    /// If the specified index already exists, the font will not be added and logs a warning message.
    /// </remarks>
    public void Add(int index, string name)
    {
        if (!fonts.TryAdd(index, name)) 
            logger.Warning($"Font with index {index} already exists in the font table.");
    }

    /// <summary>
    /// Gets the font name by its index.
    /// </summary>
    /// <param name="index">The index of the font to retrieve.</param>
    /// <returns>The name of the font at the specified index.</returns>
    public string this[int index] => fonts[index];
    
    /// <summary>
    /// Gets the number of fonts in the table.
    /// </summary>
    public int Count => fonts.Count;

    /// <summary>
    /// Finds the index of a font by its name.
    /// </summary>
    /// <param name="name">The name of the font to find.</param>
    /// <returns>The index of the font if found; otherwise, the default key value indicating not found.</returns>
    public int IndexOf(string name)

    {
        return fonts.FirstOrDefault(x => x.Value.Equals(name)).Key;
    }

    /// <summary>
    /// Removes a font by index from the font table.
    /// </summary>
    /// <param name="index">The index of the font to remove.</param>
    public void Remove(int index)
    {
        fonts.Remove(index);
    }

    /// <summary>
    /// Removes a font by name from the font table.
    /// </summary>
    /// <param name="font">The name of the font to remove.</param>
    public void Remove(string font)
    {
        fonts.Remove(fonts.FirstOrDefault(x => x.Value.Equals(font)).Key);
    }

    /// <summary>
    /// Generates a new font index that is not currently used in the font table.
    /// </summary>
    /// <returns>A new, unique font index.</returns>
    private int NewFontIndex()
    {
        return fonts.Count == 0 ? 0 : fonts.Keys.Max() + 1;
    }
}