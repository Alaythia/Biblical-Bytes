namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Represents a decoration attribute, which can be used to apply additional styling or behavior to HTML elements during RTF to HTML conversion.
/// </summary>
public class DecorationAttribute
{
    /// <summary>
    /// Gets or sets the name of the decoration attribute.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the reference or value associated with the decoration attribute.
    /// </summary>
    public string Reference { get; set; }
}