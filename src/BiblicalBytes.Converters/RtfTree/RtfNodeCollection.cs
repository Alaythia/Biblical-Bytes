using System.Collections;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfNodeCollection : CollectionBase
{
    public int Add(RtfTreeNode node)
    {
        InnerList.Add(node);

        return (InnerList.Count - 1);
    }

    public void Insert(int index, RtfTreeNode node)
    {
        InnerList.Insert(index, node);
    }

    public RtfTreeNode this[int index]
    {
        get
        {
            return (RtfTreeNode)InnerList[index];
        }
        set
        {
            InnerList[index] = value;
        }
    }

    public int IndexOf(RtfTreeNode node)
    {
        return InnerList.IndexOf(node);
    }

    public int IndexOf(RtfTreeNode node, int startIndex)
    {
        return InnerList.IndexOf(node, startIndex);
    }

    public int IndexOf(string key)
    {
        var intFoundAt = -1;

        if (InnerList.Count > 0)
        {
            for (var intIndex = 0; intIndex < InnerList.Count; intIndex++)
            {
                if (((RtfTreeNode)InnerList[intIndex]).NodeKey == key)
                {
                    intFoundAt = intIndex;
                    break;
                }
            }
        }

        return intFoundAt;
    }

    public int IndexOf(string key, int startIndex)
    {
        var intFoundAt = -1;

        if (InnerList.Count > 0)
        {
            for (var intIndex = startIndex; intIndex < InnerList.Count; intIndex++)
            {
                if (((RtfTreeNode)InnerList[intIndex]).NodeKey == key)
                {
                    intFoundAt = intIndex;
                    break;
                }
            }
        }

        return intFoundAt;
    }

    public void AddRange(RtfNodeCollection collection)
    {
        InnerList.AddRange(collection);
    }

    public void RemoveRange(int index, int count)
    {
        InnerList.RemoveRange(index, count);
    }

}