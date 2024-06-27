namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Provides methods to convert CSS style properties to their corresponding RTF code.
/// </summary>
public static class Style
{
    /// <summary>
    /// Converts text alignment CSS property value to RTF alignment code.
    /// </summary>
    /// <param name="value">The CSS value of the text alignment.</param>
    /// <returns>The RTF alignment code.</returns>
    public static string GetRtfAlignmentReference(string value)
    {
        return Alignment.GetRtfAlignmentReference(value);
    }

    /// <summary>
    /// Converts text decoration CSS property value to RTF text decoration code.
    /// </summary>
    /// <param name="value">The CSS value of the text decoration.</param>
    /// <returns>The RTF text decoration code.</returns>
    public static string GetRtfTextDecorationReference(string value)
    {
        return TextDecoration.GetRtfTextDecorationReference(value);
    }

    /// <summary>
    /// Converts color CSS property value to RTF color code.
    /// </summary>
    /// <param name="value">The CSS value of the color.</param>
    /// <returns>The RTF color code.</returns>
    public static string GetRtfReferenceColor(string value)
    {
        return Color.GetRtfReferenceColor(value);
    }

    /// <summary>
    /// Converts background-color CSS property value to RTF background color code.
    /// </summary>
    /// <param name="value">The CSS value of the background color.</param>
    /// <returns>The RTF background color code.</returns>
    public static string GetRtfReferenceBackGroundColor(string value)
    {
        return Color.GetRtfReferenceBackgroundColor(value);
    }

    /// <summary>
    /// Retrieves the RTF color table.
    /// </summary>
    /// <returns>The RTF color table.</returns>
    public static string GetRtfColorTable()
    {
        return Color.GetRtfColorTable();
    }

    /// <summary>
    /// Retrieves the RTF font table.
    /// </summary>
    /// <returns>The RTF font table.</returns>
    public static string GetRtfFontTable()
    {
        return FontFamily.GetRtfFontTable();
    }

    /// <summary>
    /// Converts font-size CSS property value to RTF font size code.
    /// </summary>
    /// <param name="value">The CSS value of the font size.</param>
    /// <returns>The RTF font size code.</returns>
    public static string GetRtfFontSizeReference(string value)
    {
        return FontSize.GetRtfFontSizeReference(value);
    }

    /// <summary>
    /// Converts font-family CSS property value to RTF font reference code.
    /// </summary>
    /// <param name="value">The CSS value of the font family.</param>
    /// <returns>The RTF font reference code.</returns>
    public static string GetRtfFontReference(string value)
    {
        value = value.Split(',')[0];
        value = value.Replace(@"'", " ");
        Console.WriteLine(value);
        return FontFamily.GetRtfReferenceFont(value);
    }

    /// <summary>
    /// Converts CSS style properties to their corresponding RTF codes.
    /// </summary>
    /// <param name="styleValue">The CSS style properties.</param>
    /// <returns>A string containing RTF codes for the given CSS style properties.</returns>
    public static string GetRtfReferencesInStyleProperty(string styleValue)
    {
        var listOfRtfReferences = "";

        foreach (var entries in styleValue.Split(';'))
        {
            string[] values = entries.Split(':');
            if (values.Length == 2)
            {

                var propertyName = values[0];
                var value = values[1];
                propertyName = propertyName.Replace(" ", "");
                value = value.Trim();

                switch (propertyName)
                {
                    case "font-family":
                         listOfRtfReferences += Style.GetRtfFontReference(value); 
                         break;
                    case "color":
                         listOfRtfReferences += Style.GetRtfReferenceColor(value); 
                         break; 
                    case "background-color":
                        listOfRtfReferences += Style.GetRtfReferenceBackGroundColor(value); 
                        break;
                    case "font-size":
                        listOfRtfReferences += Style.GetRtfFontSizeReference(value); 
                        break;
                    case "text-align":
                        listOfRtfReferences += Style.GetRtfAlignmentReference(value); 
                        break;
                    case "text-decoration":
                        listOfRtfReferences += Style.GetRtfTextDecorationReference(value);
                        break;
                }
            }

        }

        return listOfRtfReferences;
    }
}
