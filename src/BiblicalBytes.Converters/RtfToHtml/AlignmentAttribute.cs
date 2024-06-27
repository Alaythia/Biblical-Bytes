namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Represents an alignment attribute for HTML elements converted from RTF.
/// </summary>
public class AlignmentAttribute
{
    /// <summary>
    /// Gets or sets the name of the alignment attribute.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the reference value for the alignment attribute.
    /// </summary>
    public string Reference { get; set; }
}