namespace BiblicalBytes.Converters.RtfTree;

public enum RtfTokenType
{
    None = 0,
    Keyword = 1,
    Control = 2,
    Text = 3,
    Eof = 4,
    GroupStart = 5,
    GroupEnd = 6
}