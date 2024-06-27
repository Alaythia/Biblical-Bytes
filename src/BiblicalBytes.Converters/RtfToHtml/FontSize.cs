namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Provides functionality to convert font sizes between pixels, points, and predefined CSS sizes to RTF font size references.
/// </summary>
public class FontSize
{
    /// <summary>
    /// RTF reference prefix for font size.
    /// </summary>
    public const string FontSizeRtfReference = "\\fs";

    /// <summary>
    /// Conversion factor from pixels to points.
    /// </summary>
    public const double OnePixelInPoint = 0.75;

    /// <summary>
    /// Converts a font size value from CSS (pixels or points) or predefined CSS sizes to an RTF font size reference.
    /// </summary>
    /// <param name="value">The font size value in pixels, points, or predefined CSS size.</param>
    /// <returns>An RTF font size reference string.</returns>
    public static string GetRtfFontSizeReference(string value)
    {
        var isDigit = 0;
        if (value.Contains("px") || Int32.TryParse(value, out isDigit))
        {
            value = value.Replace("px", "");
            double result = 0;
            Double.TryParse(value, out result);
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

    /// <summary>
    /// A dictionary mapping predefined CSS font sizes to pixel values.
    /// </summary>
    private static readonly IDictionary<string, string> baseSizes = new Dictionary<string, string>()
    {
        {"x-small","10px"},
        {"small","13.333px"},
        {"medium","16px"},
        {"large","	18px"},
        {"x-large","24px"},
        {"xx-large","32px"},
    };

    /// <summary>
    /// Converts a font size in pixels to an RTF font size reference.
    /// </summary>
    /// <param name="valueInPixel">The font size in pixels.</param>
    /// <returns>An RTF font size reference string.</returns>
    public static string GetFontSizeReferenceInPx(double valueInPixel)
    {
        return FontSizeRtfReference + Math.Truncate((double)(valueInPixel) * OnePixelInPoint) * 2;
    }

    /// <summary>
    /// Converts a font size in points to an RTF font size reference.
    /// </summary>
    /// <param name="valueInPixel">The font size in points.</param>
    /// <returns>An RTF font size reference string.</returns>
    public static string GetFontSizeReferenceInPt(double valueInPixel)
    {
        return FontSizeRtfReference + valueInPixel * 2;
    }

}
