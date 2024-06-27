namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Represents a tag used in the conversion from RTF to HTML.
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets or sets the opening HTML tag.
    /// </summary>
    public string Opening { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RTF code corresponding to the opening HTML tag.
    /// </summary>
    public string OpeningRtf { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the closing HTML tag.
    /// </summary>
    public string Closing { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RTF code corresponding to the closing HTML tag.
    /// </summary>
    public string ClosingRtf { get; set; } = string.Empty;
}