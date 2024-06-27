namespace BiblicalBytes.Converters.RtfToHtml;

public class Format
{
    public bool Italic;
    public bool Bold;
    public bool Subscript;
    public bool Strike;
    public bool Underline;
    public bool IsLi = false;
    public bool Superscript;
    public bool HasHref = false;
    public string FontName;
    public int FontSize;
    public System.Drawing.Color ForeColor;
    public System.Drawing.Color BackColor;
    public int Margin;
    public bool SpanIsOpen;
    public bool IsOpen;
    public HorizontalAlignment Alignment;

    public Format()
    {
        Reset();
    }

    public bool CompareFontFormat(Format format)
    {
        return string.Compare(FontName, format.FontName, true) == 0 &&
               FontSize == format.FontSize &&
               ForeColor == format.ForeColor &&
               Strike == format.Strike &&
               BackColor == format.BackColor &&
               Margin == format.Margin &&
               Alignment == format.Alignment;
    }

    public void Reset()
    {
        FontName = string.Empty;
        FontSize = 0;
        Bold = false;
        Subscript = false;
        Italic = false;
        Strike = false;
        BackColor = System.Drawing.Color.White;
        Underline = false;
        Margin = 0;
        SpanIsOpen = false;
        Alignment = HorizontalAlignment.Left;
        IsOpen = false;
    }
}