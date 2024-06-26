namespace BiblicalBytes.Converters.RtfToHtml;

internal static class FontTable
{
    public static int Amount;
    public static List<string> Font = new List<string>();
        
}

internal static class FontFamily
{
    private const string RtfFontTableOpening = "{\\fonttbl";
    private const string RtfFontTableClosing = "}";

    public static string GetRtfFontTable()
    {

        return RtfFontTableOpening + GetAllFontsDeclaredInFontTable() + RtfFontTableClosing;
    }

    public static string GetRtfReferenceFont(string font)
    {
        return GetFontInFontTable(font);
    }
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
 
    public static bool VerifyIfFontExistsInFontTable(string font)
    {
        var hasThisFont = false;
        foreach (var value in FontTable.Font)
        {
            if (value==font)
                hasThisFont = true;
        }
        return hasThisFont;
    }

    public static void AddFontInFontTable(string font)
    {
        if (FontTable.Amount == 0)
        { FontTable.Font.Add("Times New Roman");
            FontTable.Amount++; }
        var rtfReferenceFont = "";
        int amountFontPosition = 0, fontsPosition = 1;
        FontTable.Amount++;

        //rtfReferenceFont = "\\f" + ColorTable.amount + "\\fcharset0" + font;
        var flag = true;
        foreach(var fontInTable in FontTable.Font )
        {
            Console.WriteLine(fontInTable + "===" + font);

            if (fontInTable == font)
            {
                flag = false;
            }
        }
        if (flag) { 
            FontTable.Font.Add(font);
        }
    }

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

    public static string GetAllFontsDeclaredInFontTable()
    {

        var fontTableContent = "";
        foreach (var value in FontTable.Font)
        {
            fontTableContent += "{\\f"+FontTable.Font.IndexOf(value)+"\\fcharset0 "+value.Trim()+";}";
        }
        return fontTableContent;
    }

}