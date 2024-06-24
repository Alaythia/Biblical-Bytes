namespace BiblicalBytes.Converters.RtfTree;

public class RtfParFormat
{
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;

    public float LeftIndentation { get; set; } = 0;

    public float RightIndentation { get; set; } = 0;
}