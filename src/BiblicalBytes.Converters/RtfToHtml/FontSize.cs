namespace BiblicalBytes.Converters.RtfToHtml;

public class FontSize
{
    public const string FontSizeRtfReference = "\\fs";
    public const double OnePixelInPoint = 0.75;
    public static string GetRtfFontSizeReference(string value)
    {
        var isDigit = 0;
        if (value.Contains("px") || Int32.TryParse(value, out isDigit))
        {
            value = value.Replace("px", "");
            double result = 0;
            Double.TryParse(value,out result);
            return GetFontSizeReferenceInPx(result);
        }
        if (value.Contains("pt"))
        {
            value = value.Replace("pt", "");
            double result = 0;
            Double.TryParse(value, out result);
            return GetFontSizeReferenceInPt(result);
        }
        else
        {
            foreach (KeyValuePair<string, string> entry in baseSizes)
            {
                if (entry.Key == value)
                    return GetRtfFontSizeReference(entry.Value);
            }
        }
        return null;
    }

    private static readonly IDictionary<string, string> baseSizes = new Dictionary<string, string>()
    {
        {"x-small","10px"},
        {"small","13.333px"},
        {"medium","16px"},
        {"large","	18px"},
        {"x-large","24px"},
        {"xx-large","32px"},                                        
    };
    public static string GetFontSizeReferenceInPx(double valueInPixel)
    {
        return FontSizeRtfReference + Math.Truncate((double)(valueInPixel) * OnePixelInPoint) *2;
    }
    public static string GetFontSizeReferenceInPt(double valueInPixel)
    {
        return FontSizeRtfReference + valueInPixel*2;
    }
        
}