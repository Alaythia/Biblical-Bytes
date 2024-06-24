namespace BiblicalBytes.Converters.RtfTree;

public class RtfFontTable
{
    private readonly Dictionary<int, string> fonts;

    public RtfFontTable()
    {
        fonts = new Dictionary<int, string>();
    }

    public void AddFont(string name)
    {
        fonts.Add(NewFontIndex(), name);
    }


    public void AddFont(int index, string name)
    {
        if (!fonts.ContainsKey(index))
        {
            fonts.Add(index, name);
        }
        else
        {
        }
    }

    public string this[int index] => fonts[index];

    public int Count => fonts.Count;

    public int IndexOf(string name)
    {
        return fonts.FirstOrDefault(x => x.Value.Equals(name)).Key;
    }

    private int NewFontIndex()
    {
        return fonts.Count == 0 ? 0 : fonts.Keys.Max() + 1;
    }
}