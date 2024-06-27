using System.Text.RegularExpressions;

namespace BiblicalBytes.Converters.RtfToHtml;

/// <summary>
/// Provides functionality to manage and convert colors for RTF to HTML conversion.
/// </summary>
public static class Color
{
    /// <summary>
    /// A dictionary mapping color names to their RGB string representations.
    /// </summary>
    private static readonly IDictionary<string, string> BaseColors = new Dictionary<string, string>()
    {
        {"black","rgb(0,0,0)"},
        {"white","rgb(255,255,255)"},
        {"red","rgb(255,0,0)"},
        {"lime","rgb(0,255,0)"},
        {"yellow","rgb(255,255,0)"},
        {"cyan","rgb(0,255,255)"},
        {"magenta","rgb(255,0,255)"},
        {"silver","rgb(192,192,192)"},
        {"gray","rgb(128,128,128)"},
        {"maroon","rgb(128,0,0)"},
        {"olive","rgb(128,128,0)"},
        {"green","rgb(0,128,0)"},
        {"purple","rgb(128,0,128)"},
        {"teal","rgb(0,128,128)"},
        {"blue","rgb(0,0,255)"},

    };

    private const string RtfColorTableOpening = "{\\colortbl";
    private const string RtfColorTableClosing = "}";

    /// <summary>
    /// Generates the RTF color table from the base colors.
    /// </summary>
    /// <returns>A string representing the RTF color table.</returns>
    public static string GetRtfColorTable()
    {
        return RtfColorTableOpening + GetAllColorsDeclaredInColorTable() + RtfColorTableClosing;
    }

    /// <summary>
    /// Converts a color name or RGB/Hex value to its RTF color reference.
    /// </summary>
    /// <param name="color">The color name, RGB, or Hex value.</param>
    /// <returns>The RTF color reference string.</returns>
    public static string GetRtfReferenceColor(string color)
    {
        foreach (KeyValuePair<string, string> entry in BaseColors)
        {
            if (entry.Key == color.ToLower())
                color = entry.Value;
        }
        if (color.Contains("rgb"))
            return GetColorInColorTable(GetRgbValues(color));

        if (color.Contains("#"))
            return GetColorInColorTable(ConvertColorInHexToRgb(color));

        return null;
    }

    /// <summary>
    /// Converts a color name or RGB/Hex value to its RTF background color reference.
    /// </summary>
    /// <param name="color">The color name, RGB, or Hex value.</param>
    /// <returns>The RTF background color reference string.</returns>
    public static string GetRtfReferenceBackgroundColor(string color)
    {
        foreach (KeyValuePair<string, string> entry in BaseColors)
        {
            if (entry.Key == color.ToLower())
                color = entry.Value;
        }
        if (color.Contains("rgb"))
            return GetBackColorInColorTable(GetRgbValues(color));

        if (color.Contains("#"))
            return GetBackColorInColorTable(ConvertColorInHexToRgb(color));

        return null;
    }

    /// <summary>
    /// Extracts RGB values from a color string.
    /// </summary>
    /// <param name="color">The color string in RGB format.</param>
    /// <returns>An array of double values representing the RGB components.</returns>
    public static double[] GetRgbValues(string color)
    {
        color = Regex.Replace(color, "[\\])}[{(rgb:; ]", "");
        return Array.ConvertAll(color.Split(','), Double.Parse);
    }

    /// <summary>
    /// Converts a Hex color value to its RGB components.
    /// </summary>
    /// <param name="hexColor">The Hex color value.</param>
    /// <returns>An array of double values representing the RGB components.</returns>
    public static double[] ConvertColorInHexToRgb(string hexColor)
    {
        Console.WriteLine("-----");
        Console.WriteLine(hexColor);
        var rgb = new double[3];
        hexColor = Regex.Replace(hexColor, "[#; ]", "");
        Console.WriteLine(hexColor);
        hexColor = (hexColor.Length == 3) ? hexColor[0] + "" + hexColor[0] + "" + hexColor[1] + "" + hexColor[1] + "" + hexColor[2] + "" + hexColor[2] : hexColor;
        Console.WriteLine(hexColor);

         
        rgb[2] = Convert.ToInt32((hexColor[4].ToString()+hexColor[5].ToString()).ToString(), 16);
        rgb[1] = Convert.ToInt32((hexColor[2].ToString()+hexColor[3].ToString()).ToString(), 16);
        rgb[0] = Convert.ToInt32((hexColor[0].ToString()+hexColor[1].ToString()).ToString(), 16);

        Console.WriteLine(rgb[0]+"-r"+rgb[1]+"-g"+rgb[2]);
        Console.WriteLine("-----");

        return rgb;
    }

    /// <summary>
    /// Adds a color to the color table and returns its RTF color reference.
    /// </summary>
    /// <param name="rgb">The RGB components of the color.</param>
    /// <returns>The RTF color reference string.</returns>

    public static string GetColorInColorTable(double[] rgb)
    {
        if (VerifyIfColorExistsInColorTable(rgb, "fore"))
            return GetRtfReferenceColorInColorTable(rgb, "fore");
        else
        {
            AddColorInColorTable(rgb, "fore");
            return GetRtfReferenceColorInColorTable(rgb, "fore");
        }
    }

    /// <summary>
    /// Adds a color to the color table and returns its RTF background color reference.
    /// </summary>
    /// <param name="rgb">The RGB components of the color.</param>
    /// <returns>The RTF background color reference string.</returns>
    public static string GetBackColorInColorTable(double[] rgb)
    {
        if (VerifyIfColorExistsInColorTable(rgb, "back"))
            return GetRtfReferenceColorInColorTable(rgb, "back");
        else
        {
            AddColorInColorTable(rgb, "back");
            return GetRtfReferenceColorInColorTable(rgb, "back");
        }
    }

    /// <summary>
    /// Verifies if a color exists in the color table.
    /// </summary>
    /// <param name="rgb">The RGB components of the color.</param>
    /// <param name="type">The type of color reference ("fore" for foreground, "back" for background).</param>
    /// <returns>true if the color exists; otherwise, false.</returns>
    public static bool VerifyIfColorExistsInColorTable(double[] rgb, string type)
    {
        var hasThisColor = false; var colorsPosition = 1;
        var style = "";
        if (type == "fore")
        {
            style = "cf";
        }
        else if (type == "back")
        {
            style = "highlight";
        }
        foreach (string[] value in ColorTable.Colors)
        {
            if (value[0] == rgb[0].ToString() && value[1] == rgb[1].ToString() && value[2] == rgb[2].ToString())
                hasThisColor = true;
        }
        return hasThisColor;
    }

    /// <summary>
    /// Adds a color to the color table.
    /// </summary>
    /// <param name="rgb">The RGB components of the color.</param>
    /// <param name="type">The type of color reference ("fore" for foreground, "back" for background).</param>
    public static void AddColorInColorTable(double[] rgb, string type)
    {
        var rtfReferenceColor = "";
        int amountColorPosition = 0, colorsPosition = 1;
        ColorTable.Amount++;
        Console.WriteLine(rgb[0] + rgb[1] + rgb[2] + ColorTable.Amount);
        rtfReferenceColor = "\\cf" + ColorTable.Amount;
        ColorTable.Colors.Add([rgb[0].ToString(), rgb[1].ToString(), rgb[2].ToString(), rtfReferenceColor]);
    }

    /// <summary>
    /// Retrieves the RTF color reference for a color in the color table.
    /// </summary>
    /// <param name="rgb">The RGB components of the color.</param>
    /// <param name="type">The type of color reference ("fore" for foreground, "back" for background).</param>
    /// <returns>The RTF color reference string.</returns>
    public static string GetRtfReferenceColorInColorTable(double[] rgb, string type)
    {
        var rtfReferenceColor = "";
        var style = "";
           
        foreach (string[] value in ColorTable.Colors)
        {
            if (value[0] == rgb[0].ToString() && value[1] == rgb[1].ToString() && value[2] == rgb[2].ToString())
                rtfReferenceColor = value[3];
        }
        if (type == "fore")
        {
            rtfReferenceColor.Replace("highlight","cf");
        }
        else if (type == "back")
        {
            Console.WriteLine(rtfReferenceColor);
            rtfReferenceColor =  rtfReferenceColor.Replace("cf", "highlight");

            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Console.WriteLine(rtfReferenceColor);

        }
        return rtfReferenceColor;
    }

    /// <summary>
    /// Compiles all colors declared in the color table into a single string.
    /// </summary>
    /// <returns>A string representing all colors in the RTF color table format.</returns>
    public static string GetAllColorsDeclaredInColorTable()
    {
        var colorTableContent = "";
        foreach (string[] value in ColorTable.Colors)
        {
            colorTableContent += "\\red" + value[0] + "\\green" + value[1] + "\\blue" + value[2] + " ;";
        }
        return colorTableContent;
    }

}