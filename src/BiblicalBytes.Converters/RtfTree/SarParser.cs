namespace BiblicalBytes.Converters.RtfTree;

public abstract class SarParser
{
    public abstract void StartRtfDocument();
    public abstract void EndRtfDocument();
    public abstract void StartRtfGroup();
    public abstract void EndRtfGroup();
    public abstract void RtfKeyword(string key, bool hasParameter, int parameter);
    public abstract void RtfControl(string key, bool hasParameter, int parameter);
    public abstract void RtfText(string text);
}