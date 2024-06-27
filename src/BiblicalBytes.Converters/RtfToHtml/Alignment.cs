namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Provides functionality to map HTML alignment properties to their corresponding RTF alignment control words.
/// </summary>
public static class Alignment
{
    /// <summary>
    /// An array of <see cref="AlignmentAttribute"/> objects representing the mapping of HTML alignment properties to RTF alignment control words.
    /// </summary>
    public static readonly AlignmentAttribute[] AlignmentReferenceList =
    [
        new AlignmentAttribute() { Name= "center",  Reference="\\qc" },
        new AlignmentAttribute() { Name="left",     Reference="\\ql" },
        new AlignmentAttribute() { Name="right",    Reference="\\qr" },
        new AlignmentAttribute() { Name="justify",  Reference="\\qj" }
    ];

    /// <summary>
    /// Retrieves the RTF alignment control word for a given HTML alignment property name.
    /// </summary>
    /// <param name="propertyName">The name of the HTML alignment property.</param>
    /// <returns>The RTF alignment control word if found; otherwise, an empty string.</returns>
    public static string GetRtfAlignmentReference(string propertyName)
    {
        var alignmentReference = "";
        foreach (var element in AlignmentReferenceList)
        {
            if (element.Name == propertyName.Trim())
            {
                alignmentReference = element.Reference;
            }
        }

        return alignmentReference;
    }
}