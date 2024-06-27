namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Represents the formatting attributes for an element being converted from RTF to HTML.
/// </summary>
public class Format
{
    /// <summary>Indicates whether the text is italicized.</summary>
    public bool Italic;
    /// <summary>Indicates whether the text is bold.</summary>
    public bool Bold;
    /// <summary>Indicates whether the text is subscript.</summary>
    public bool Subscript;
    /// <summary>Indicates whether the text has a strikethrough.</summary>
    public bool Strike;
    /// <summary>Indicates whether the text is underlined.</summary>
    public bool Underline;
    /// <summary>Indicates whether the element is a list item.</summary>
    public bool IsLi = false;
    /// <summary>Indicates whether the text is superscript.</summary>
    public bool Superscript;
    /// <summary>Indicates whether the element contains a hyperlink reference.</summary>
    public bool HasHref = false;
    /// <summary>The name of the font.</summary>
    public string FontName;
    /// <summary>The size of the font.</summary>
    public int FontSize;
    /// <summary>The color of the text.</summary>
    public System.Drawing.Color ForeColor;
    /// <summary>The background color of the text.</summary>
    public System.Drawing.Color BackColor;
    /// <summary>The margin of the element.</summary>
    public int Margin;
    /// <summary>Indicates whether a span element is open.</summary>
    public bool SpanIsOpen;
    /// <summary>Indicates whether the format is currently applied.</summary>
    public bool IsOpen;
    /// <summary>The horizontal alignment of the text.</summary>
    public HorizontalAlignment Alignment;

    /// <summary>
    /// Initializes a new instance of the <see cref="Format"/> class.
    /// </summary>
    public Format()
    {
        Reset();
    }

    /// <summary>
    /// Compares the font format with another format object.
    /// </summary>
    /// <param name="format">The format to compare with.</param>
    /// <returns>true if the formats are equivalent; otherwise, false.</returns>
    public bool CompareFontFormat(Format format)
    {
        return string.Compare(FontName, format.FontName, true) == 0 &&
               FontSize == format.FontSize &&
               ForeColor == format.ForeColor &&
               Strike == format.Strike &&
               BackColor == format.BackColor &&
               Margin == format.Margin &&
               Alignment == format.Alignment;
    }

    /// <summary>
    /// Resets the format to its default state.
    /// </summary>
    public void Reset()
    {
        FontName = string.Empty;
        FontSize = 0;
        Bold = false;
        Subscript = false;
        Italic = false;
        Strike = false;
        BackColor = System.Drawing.Color.White;
        Underline = false;
        Margin = 0;
        SpanIsOpen = false;
        Alignment = HorizontalAlignment.Left;
        IsOpen = false;
    }
}
