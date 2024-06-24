namespace BiblicalBytes.Converters.RtfTree;

public class RtfStyleSheet
{
    public int Index { get; set; } = 0;

    public string Name { get; set; } = "";

    public RtfStyleSheetType Type { get; set; } = RtfStyleSheetType.Paragraph;

    public bool Additive { get; set; } = false;

    public int BasedOn { get; set; } = -1;

    public int Next { get; set; } = -1;

    public bool AutoUpdate { get; set; } = false;

    public bool Hidden { get; set; } = false;

    public int Link { get; set; } = -1;

    public bool Locked { get; set; } = false;

    public bool Personal { get; set; } = false;

    public bool Compose { get; set; } = false;

    public bool Reply { get; set; } = false;

    public int Styrsid { get; set; } = -1;

    public bool SemiHidden { get; set; } = false;

    public RtfNodeCollection KeyCode { get; set; } = null;

    public RtfNodeCollection Formatting { get; set; } = null;

    public RtfStyleSheet()
    {
        KeyCode = null;
        Formatting = new RtfNodeCollection();
    }

}