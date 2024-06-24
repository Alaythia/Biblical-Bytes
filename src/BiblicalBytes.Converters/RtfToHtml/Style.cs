namespace BiblicalBytes.Converters.RtfToHtml;

internal static class Style
{
    public static string GetRtfAlignmentReference(string value)
    {
        return Alignment.GetRtfAlignmentReference(value);

    }
    public static string GetRtfTextDecorationReference(string value)
    {
        return TextDecoration.GetRtfTextDecorationReference(value);

    }
        
    public static string GetRtfReferenceColor(string value)
    {
        return Color.GetRtfReferenceColor(value);
    }
       
    public static string GetRtfReferenceBackGroundColor(string value)
    {
        return Color.GetRtfReferenceBackgroundColor(value);
    }
    public static string GetRtfColorTable()
    {
        return Color.GetRtfColorTable();
    }
    public static string GetRtfFontTable()
    {
        return FontFamily.GetRtfFontTable();
    }
    public static string GetRtfFontSizeReference(string value)
    {
        return FontSize.GetRtfFontSizeReference(value);
    }
    public static string GetRtfFontReference(string value)
    {
        value = value.Split(',')[0];
        value = value.Replace(@"'", " ");
        Console.WriteLine(value);
        return FontFamily.GetRtfReferenceFont(value);
    }
    public static string GetRtfReferencesInStyleProperty(string styleValue)
    {
        var value = "";
        var propertyName = "";
        var listOfRtfReferences = "";

        foreach (var entries in styleValue.Split(';'))
        {
            string[] values = entries.Split(':');
            if (values.Length == 2)
            {

                propertyName = values[0];
                value = values[1];
                propertyName = propertyName.Replace(" ", "");
                value = value.Trim();

                switch (propertyName)
                {
                    case "font-family":

                    { listOfRtfReferences += Style.GetRtfFontReference(value); break; }
                    
                    case "color":
                    { listOfRtfReferences += Style.GetRtfReferenceColor(value); break; }
                    case "background-color":
                    { listOfRtfReferences += Style.GetRtfReferenceBackGroundColor(value); break; }
                    case "font-size":
                    { listOfRtfReferences += Style.GetRtfFontSizeReference(value); break; }
                    case "text-align":
                    { listOfRtfReferences += Style.GetRtfAlignmentReference(value); break; }
                    case "text-decoration":
                    { listOfRtfReferences += Style.GetRtfTextDecorationReference(value); break; }
                }
            }
            
        }



        return listOfRtfReferences;
    }
}