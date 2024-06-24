namespace BiblicalBytes.Style;

public interface IStyleTable<T>
{
    /// <summary>
    /// Adds a new font with an automatically assigned index to the font table.
    /// </summary>
    /// <param name="obj">The object to add.</param>
    void Add(T obj);

    /// <summary>
    /// Adds a font with a specified index to the font table.
    /// </summary>
    /// <param name="index">The index at which to add the font.</param>
    /// <param name="obj">The object to add.</param>
    /// <remarks>
    /// If the specified index already exists, the font will not be added and logs a warning message.
    /// </remarks>
    void Add(int index, T obj);

    /// <summary>
    /// Gets the object by its index.
    /// </summary>
    /// <param name="index">The index of the font to retrieve.</param>
    /// <returns>The object at the specified index.</returns>
    T this[int index] { get; }

    /// <summary>
    /// Gets the number of objects in the table.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Finds the index of a font by its name.
    /// </summary>
    /// <param name="obj">The object to find.</param>
    /// <returns>The index of the object if found; otherwise, the default key value indicating not found.</returns>
    int IndexOf(T obj);
}