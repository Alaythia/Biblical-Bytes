namespace BiblicalBytes.Converters.RtfTree;

public class RtfStyleSheet
{
    public int Index { get; set; }

    public string Name { get; set; } = "";

    public RtfStyleSheetType Type { get; set; } = RtfStyleSheetType.Paragraph;

    public bool Additive { get; set; }

    public int BasedOn { get; set; } = -1;

    public int Next { get; set; } = -1;

    public bool AutoUpdate { get; set; }

    public bool Hidden { get; set; }

    public int Link { get; set; } = -1;

    public bool Locked { get; set; }

    public bool Personal { get; set; }

    public bool Compose { get; set; }

    public bool Reply { get; set; }

    public int Styrsid { get; set; } = -1;

    public bool SemiHidden { get; set; }

    public RtfNodeCollection? KeyCode { get; set; }

    public RtfNodeCollection? Formatting { get; set; }

    public RtfStyleSheet()
    {
        KeyCode = null;
        Formatting = [];
    }

}