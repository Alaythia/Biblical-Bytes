using System.Collections;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfStyleSheetTable
{
    private readonly Dictionary<int, RtfStyleSheet> stylesheets;

    public RtfStyleSheetTable()
    {
        stylesheets = new Dictionary<int, RtfStyleSheet>();
    }

    public void AddStyleSheet(RtfStyleSheet ss)
    {
        ss.Index = NewStyleSheetIndex();

        stylesheets.Add(ss.Index, ss);
    }

    public void AddStyleSheet(int index, RtfStyleSheet ss)
    {
        ss.Index = index;

        stylesheets.Add(index, ss);
    }

    public void RemoveStyleSheet(int index)
    {
        stylesheets.Remove(index);
    }

    public void RemoveStyleSheet(RtfStyleSheet ss)
    {
        stylesheets.Remove(ss.Index);
    }

    public RtfStyleSheet GetStyleSheet(int index)
    {
        return stylesheets[index];
    }

    public RtfStyleSheet this[int index]
    {
        get
        {
            return stylesheets[index];
        }
    }

    public int Count
    {
        get
        {
            return stylesheets.Count;
        }
    }

    public int IndexOf(string name)
    {
        var intIndex = -1;
        IEnumerator fntIndex = stylesheets.GetEnumerator();

        fntIndex.Reset();
        while (fntIndex.MoveNext())
        {
            if (((KeyValuePair<int, RtfStyleSheet>)fntIndex.Current).Value.Name.Equals(name))
            {
                intIndex = (int)((KeyValuePair<int, RtfStyleSheet>)fntIndex.Current).Key;
                break;
            }
        }

        return intIndex;
    }

    private int NewStyleSheetIndex()
    {
        var intIndex = -1;
        IEnumerator fntIndex = stylesheets.GetEnumerator();

        fntIndex.Reset();
        while (fntIndex.MoveNext())
        {
            if ((int)((KeyValuePair<int, RtfStyleSheet>)fntIndex.Current).Key > intIndex)
                intIndex = (int)((KeyValuePair<int, RtfStyleSheet>)fntIndex.Current).Key;
        }

        return (intIndex + 1);
    }
}