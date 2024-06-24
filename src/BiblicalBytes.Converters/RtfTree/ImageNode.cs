using System.Drawing;
using System.Globalization;
using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class ImageNode : RtfTreeNode
{
    private byte[] data;

    public ImageNode(RtfTreeNode node)
    {
        if(node != null)
        {
            NodeKey = node.NodeKey;
            HasParameter = node.HasParameter;
            Parameter = node.Parameter;
            ParentNode = node.ParentNode;
            RootNode = node.RootNode;
            NodeType = node.NodeType;

            ChildNodes = new RtfNodeCollection();
            ChildNodes.AddRange(node.ChildNodes);

            GetImageData();
        }
    }

    public string HexData
    {
        get
        {
            return SelectSingleChildNode(RtfNodeType.Text).NodeKey;
        }
    }

    public System.Drawing.Imaging.ImageFormat ImageFormat
    { 
        get 
        {
            if (SelectSingleChildNode("jpegblip") != null)
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            else if (SelectSingleChildNode("pngblip") != null)
                return System.Drawing.Imaging.ImageFormat.Png;
            else if (SelectSingleChildNode("emfblip") != null)
                return System.Drawing.Imaging.ImageFormat.Emf;
            else if (SelectSingleChildNode("wmetafile") != null)
                return System.Drawing.Imaging.ImageFormat.Wmf;
            else if (SelectSingleChildNode("dibitmap") != null || SelectSingleChildNode("wbitmap") != null)
                return System.Drawing.Imaging.ImageFormat.Bmp;
            else
                return null;
        }
    }

    public int Width
    {
        get
        {
            var node = SelectSingleChildNode("picw");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public int Height
    {
        get
        {
            var node = SelectSingleChildNode("pich");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public int DesiredWidth
    {
        get
        {
            var node = SelectSingleChildNode("picwgoal");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public int DesiredHeight
    {
        get
        {
            var node = SelectSingleChildNode("pichgoal");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public int ScaleX
    {
        get
        {
            var node = SelectSingleChildNode("picscalex");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public int ScaleY
    {
        get
        {
            var node = SelectSingleChildNode("picscaley");

            if (node != null)
                return node.Parameter;
            else
                return -1;
        }
    }

    public Bitmap Bitmap
    {
        get
        {
            var stream = new MemoryStream(GetByteData(), 0, data.Length);
            return new Bitmap(stream);
        }
    }

    public byte[] GetByteData()
    {
        return data;
    }

    public void SaveImage(string filePath)
    {
        if (data != null)
        {
            var stream = new MemoryStream(GetByteData(), 0, data.Length);

            var bitmap = new Bitmap(stream);
            bitmap.Save(filePath, ImageFormat);
        }
    }

    public void SaveImage(string filePath, System.Drawing.Imaging.ImageFormat format)
    {
        if (data != null)
        {
            var stream = new MemoryStream(data, 0, data.Length);

            var bitmap = new Bitmap(stream);
            bitmap.Save(filePath, format);
        }
    }

    private void GetImageData()
    {
        var text = "";

        if (FirstChild.NodeKey == "pict")
        {
            text = SelectSingleChildNode(RtfNodeType.Text).NodeKey;

            var dataSize = text.Length / 2;
            data = new byte[dataSize];

            var sbaux = new StringBuilder(2);

            for (var i = 0; i < text.Length; i++)
            {
                sbaux.Append(text[i]);

                if (sbaux.Length == 2)
                {
                    data[i / 2] = byte.Parse(sbaux.ToString(), NumberStyles.HexNumber);
                    sbaux.Remove(0, 2);
                }
            }
        }
    }

}