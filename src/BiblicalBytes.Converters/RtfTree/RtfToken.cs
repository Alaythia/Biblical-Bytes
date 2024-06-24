namespace BiblicalBytes.Converters.RtfTree;

public class RtfToken
{
    public RtfTokenType Type { get; set; }

    public string Key { get; set; }

    public bool HasParameter { get; set; }

    public int Parameter { get; set; }

    public RtfToken()
    {
        Type = RtfTokenType.None;
        Key = "";
                
    }
}