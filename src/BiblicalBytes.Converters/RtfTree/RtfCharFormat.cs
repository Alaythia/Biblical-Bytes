using System.Drawing;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfCharFormat
{
    public bool Bold { get; set; } = false;

    public bool Italic { get; set; } = false;

    public bool Underline { get; set; } = false;

    public string Font { get; set; } = "Arial";

    public int Size { get; set; } = 10;

    public Color Color { get; set; } = Color.Black;
}