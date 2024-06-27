namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Provides functionality to map CSS text decoration properties to their corresponding RTF codes.
/// </summary>
public static class TextDecoration
{
    /// <summary>
    /// A list of text decoration attributes and their corresponding RTF references.
    /// </summary>
    public static DecorationAttribute[] TextDecorationReferenceList =
    [
        new DecorationAttribute() { Name= "underline",   Reference="\\ul" },
        new DecorationAttribute() { Name="overline",     Reference="\\ol" },
        new DecorationAttribute() { Name="line-through",    Reference="\\strike" }
    ];

    /// <summary>
    /// Retrieves the RTF code for a given CSS text decoration property.
    /// </summary>
    /// <param name="propertyName">The name of the CSS text decoration property.</param>
    /// <returns>The corresponding RTF code if found; otherwise, an empty string.</returns>
    public static string GetRtfTextDecorationReference(string propertyName)
    {
        var alignmentReference = "";
        foreach (var element in TextDecorationReferenceList)
        {
            if (element.Name == propertyName.Trim())
            {
                alignmentReference = element.Reference;
            }
        }

        return alignmentReference;
    }
}