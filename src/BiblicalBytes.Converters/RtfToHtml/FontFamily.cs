namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Manages the font table for RTF conversion, allowing for the addition and retrieval of fonts.
/// </summary>
public static class FontTable
{
    /// <summary>
    /// The total number of fonts in the font table.
    /// </summary>
    public static int Amount;

    /// <summary>
    /// A list of font names in the font table.
    /// </summary>
    public static List<string> Font = new List<string>();
}

/// <summary>
/// Provides methods for working with the RTF font table, including adding fonts and generating RTF font references.
/// </summary>
internal static class FontFamily
{
    private const string RtfFontTableOpening = "{\\fonttbl";
    private const string RtfFontTableClosing = "}";

    /// <summary>
    /// Generates the RTF font table from the fonts added.
    /// </summary>
    /// <returns>A string representing the RTF font table.</returns>
    public static string GetRtfFontTable()
    {
        return RtfFontTableOpening + GetAllFontsDeclaredInFontTable() + RtfFontTableClosing;
    }

    /// <summary>
    /// Retrieves the RTF reference for a specified font, adding the font to the table if necessary.
    /// </summary>
    /// <param name="font">The name of the font.</param>
    /// <returns>The RTF font reference.</returns>
    public static string GetRtfReferenceFont(string font)
    {
        return GetFontInFontTable(font);
    }

    /// <summary>
    /// Adds a font to the font table if it does not already exist and returns its RTF reference.
    /// </summary>
    /// <param name="font">The name of the font to add.</param>
    /// <returns>The RTF font reference.</returns>
    public static string GetFontInFontTable(string font)
    {
        if (VerifyIfFontExistsInFontTable(font))
            return GetRtfReferenceFontInFontTable(font);
        else
        {
            AddFontInFontTable(font);
            return GetRtfReferenceFontInFontTable(font);
        }
    }

    /// <summary>
    /// Checks if a font exists in the font table.
    /// </summary>
    /// <param name="font">The name of the font to check.</param>
    /// <returns>true if the font exists; otherwise, false.</returns>
    public static bool VerifyIfFontExistsInFontTable(string font)
    {
        var hasThisFont = false;
        foreach (var value in FontTable.Font)
        {
            if (value == font)
                hasThisFont = true;
        }
        return hasThisFont;
    }

    /// <summary>
    /// Adds a font to the font table.
    /// </summary>
    /// <param name="font">The name of the font to add.</param>
    public static void AddFontInFontTable(string font)
    {
        if (FontTable.Amount == 0)
        {
            FontTable.Font.Add("Times New Roman");
            FontTable.Amount++;
        }
        var rtfReferenceFont = "";
        int amountFontPosition = 0, fontsPosition = 1;
        FontTable.Amount++;

        var flag = true;
        foreach (var fontInTable in FontTable.Font)
        {
            Console.WriteLine(fontInTable + "===" + font);

            if (fontInTable == font)
            {
                flag = false;
            }
        }
        if (flag)
        {
            FontTable.Font.Add(font);
        }
    }

    /// <summary>
    /// Retrieves the RTF reference for a font already in the font table.
    /// </summary>
    /// <param name="font">The name of the font.</param>
    /// <returns>The RTF font reference.</returns>
    public static string GetRtfReferenceFontInFontTable(string font)
    {
        var rtfReferenceFont = "";

        foreach (var value in FontTable.Font)
        {
            Console.WriteLine(value + "----" + font);
            if (value == font)
                rtfReferenceFont = "\\f" + FontTable.Font.IndexOf(value);
        }

        return rtfReferenceFont;
    }

    /// <summary>
    /// Compiles all fonts declared in the font table into a single string.
    /// </summary>
    /// <returns>A string representing all fonts in the RTF font table format.</returns>
    public static string GetAllFontsDeclaredInFontTable()
    {
        var fontTableContent = "";
        foreach (var value in FontTable.Font)
        {
            fontTableContent += "{\\f" + FontTable.Font.IndexOf(value) + "\\fcharset0 " + value.Trim() + ";}";
        }
        return fontTableContent;
    }
}
