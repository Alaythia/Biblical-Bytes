using System.Globalization;
using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class ObjectNode : RtfTreeNode
{
    private byte[] objdata;

    public ObjectNode(RtfTreeNode node)
    {
        if(node != null)
        {
            NodeKey = node.NodeKey;
            HasParameter = node.HasParameter;
            Parameter= node.Parameter;
            ParentNode = node.ParentNode;
            RootNode = node.RootNode;
            NodeType = node.NodeType;

            ChildNodes = new RtfNodeCollection();
            ChildNodes.AddRange(node.ChildNodes);

            GetObjectData();
        }
    }

    public string ObjectType
    {
        get 
        {
            if (SelectSingleChildNode("objemb") != null)
                return "objemb";
            if (SelectSingleChildNode("objlink") != null)
                return "objlink";
            if (SelectSingleChildNode("objautlink") != null)
                return "objautlink";
            if (SelectSingleChildNode("objsub") != null)
                return "objsub";
            if (SelectSingleChildNode("objpub") != null)
                return "objpub";
            if (SelectSingleChildNode("objicemb") != null)
                return "objicemb";
            if (SelectSingleChildNode("objhtml") != null)
                return "objhtml";
            if (SelectSingleChildNode("objocx") != null)
                return "objocx";
            else
                return "";
        }
    }

    public string ObjectClass
    {
        get
        {
            var node = SelectSingleNode("objclass");

            if (node != null)
                return node.NextSibling.NodeKey;
            else
                return "";
        }
    }

    public RtfTreeNode ResultNode
    {
        get
        {
            var node = SelectSingleNode("result");

            if (node != null)
                node = node.ParentNode;

            return node;
        }
    }

    public string HexData
    {
        get
        {
            var text = "";

            var objdataNode = SelectSingleNode("objdata");

            if (objdataNode != null)
            {
                text = objdataNode.ParentNode.LastChild.NodeKey;
            }

            return text;				
        }
    }

    public byte[] GetByteData()
    {
        return objdata;
    }

    private void GetObjectData()
    {
        var text = "";

        if (FirstChild.NodeKey == "object")
        {
            var objdataNode = SelectSingleNode("objdata");

            if (objdataNode != null)
            {
                text = objdataNode.ParentNode.LastChild.NodeKey;

                var dataSize = text.Length / 2;
                objdata = new byte[dataSize];

                var sbaux = new StringBuilder(2);

                for (var i = 0; i < text.Length; i++)
                {
                    sbaux.Append(text[i]);

                    if (sbaux.Length == 2)
                    {
                        objdata[i / 2] = byte.Parse(sbaux.ToString(), NumberStyles.HexNumber);
                        sbaux.Remove(0, 2);
                    }
                }
            }
        }
    }

}