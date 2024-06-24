using System.Drawing;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfColorTable
{
    private readonly List<int> colors;

    public RtfColorTable()
    {
        colors = new List<int>();
    }

    public void AddColor(Color color)
    {
        colors.Add(color.ToArgb());
    }

    public Color this[int index]
    {
        get
        {
            return Color.FromArgb(colors[index]);
        }
    }

    public int Count
    {
        get
        {
            return colors.Count;
        }
    }

    public int IndexOf(Color color)
    {
        return colors.IndexOf(color.ToArgb());
    }
}