using System.Drawing;
using System.Globalization;
using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfDocument
{
    private readonly Encoding encoding;

    private readonly RtfFontTable fontTable;

    private readonly RtfColorTable colorTable;

    private readonly RtfTreeNode mainGroup;

    private RtfCharFormat currentFormat;

    private readonly RtfParFormat currentParFormat;

    private readonly RtfDocumentFormat docFormat;

    public RtfDocument(Encoding enc)
    {
        encoding = enc;

        fontTable = new RtfFontTable();
        fontTable.AddFont("Arial");   

        colorTable = new RtfColorTable();
        colorTable.AddColor(Color.Black);   

        currentFormat = null;
        currentParFormat = new RtfParFormat();
        docFormat = new RtfDocumentFormat();

        mainGroup = new RtfTreeNode(RtfNodeType.Group);

        InitializeTree();
    }

    public RtfDocument() : this(Encoding.Default)
    {
    }

    public void Save(string path)
    {
        var tree = GetTree();
        tree.SaveRtf(path);
    }

    public void AddText(string text, RtfCharFormat format)
    {
        UpdateFontTable(format);
        UpdateColorTable(format);

        UpdateCharFormat(format);

        InsertText(text);
    }

    public void AddText(string text)
    {
        InsertText(text);
    }

    public void AddNewLine(int n)
    {
        for(var i=0; i<n; i++)
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "line", false, 0));
    }

    public void AddNewLine()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "line", false, 0));
    }

    public void AddNewParagraph()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "par", false, 0));
    }

    public void AddNewParagraph(int n)
    {
        for (var i = 0; i < n; i++)
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "par", false, 0));
    }

    public void AddNewParagraph(RtfParFormat format)
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "par", false, 0));

        UpdateParFormat(format);
    }

    public void AddImage(string path, int width, int height)
    {
        FileStream fStream = null;
        BinaryReader br = null;

        try
        {
            byte[] data = null;

            var fInfo = new FileInfo(path);
            var numBytes = fInfo.Length;

            fStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fStream);

            data = br.ReadBytes((int)numBytes);

            var hexdata = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                hexdata.Append(GetHexa(data[i]));
            }

            var img = Image.FromFile(path);

            var imgGroup = new RtfTreeNode(RtfNodeType.Group);
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pict", false, 0));

            var format = "";
            if (path.ToLower().EndsWith("wmf"))
                format = "emfblip";
            else
                format = "jpegblip";

            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, format, false, 0));
                    
                    
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "picw", true, img.Width * 20));
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pich", true, img.Height * 20));
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "picwgoal", true, width * 20));
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pichgoal", true, height * 20));
            imgGroup.AppendChild(new RtfTreeNode(RtfNodeType.Text, hexdata.ToString(), false, 0));

            mainGroup.AppendChild(imgGroup);
        }
        finally
        {
            if (br != null) br.Close();
            if (fStream != null) fStream.Close();
        }
    }

    public void ResetCharFormat()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "plain", false, 0));
    }

    public void ResetParFormat()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pard", false, 0));
    }

    public void ResetFormat()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pard", false, 0));
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "plain", false, 0));
    }

    public void UpdateDocFormat(RtfDocumentFormat format)
    {
        docFormat.MarginL = format.MarginL;
        docFormat.MarginR = format.MarginR;
        docFormat.MarginT = format.MarginT;
        docFormat.MarginB = format.MarginB;
    }

    public void UpdateCharFormat(RtfCharFormat format)
    {
        if (currentFormat != null)
        {
            SetFormatColor(format.Color);
            SetFormatSize(format.Size);
            SetFormatFont(format.Font);

            SetFormatBold(format.Bold);
            SetFormatItalic(format.Italic);
            SetFormatUnderline(format.Underline);
        }
        else   
        {
            var indColor = colorTable.IndexOf(format.Color);

            if (indColor == -1)
            {
                colorTable.AddColor(format.Color);
                indColor = colorTable.IndexOf(format.Color);
            }

            var indFont = fontTable.IndexOf(format.Font);

            if (indFont == -1)
            {
                fontTable.AddFont(format.Font);
                indFont = fontTable.IndexOf(format.Font);
            }

            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "cf", true, indColor));
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "fs", true, format.Size * 2));
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "f", true, indFont));

            if (format.Bold)
                mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "b", false, 0));

            if (format.Italic)
                mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "i", false, 0));

            if (format.Underline)
                mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "ul", false, 0));

            currentFormat = new RtfCharFormat();
            currentFormat.Color = format.Color;
            currentFormat.Size = format.Size;
            currentFormat.Font = format.Font;
            currentFormat.Bold = format.Bold;
            currentFormat.Italic = format.Italic;
            currentFormat.Underline = format.Underline;
        }
    }

    public void UpdateParFormat(RtfParFormat format)
    {
        SetAlignment(format.Alignment);
        SetLeftIndentation(format.LeftIndentation);
        SetRightIndentation(format.RightIndentation);
    }

    public void SetAlignment(TextAlignment align)
    {
        if (currentParFormat.Alignment != align)
        {
            var keyword = "";

            switch (align)
            { 
                case TextAlignment.Left:
                    keyword = "ql";
                    break;
                case TextAlignment.Right:
                    keyword = "qr";
                    break;
                case TextAlignment.Centered:
                    keyword = "qc";
                    break;
                case TextAlignment.Justified:
                    keyword = "qj";
                    break;
            }

            currentParFormat.Alignment = align;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, keyword, false, 0));
        }
    }

    public void SetLeftIndentation(float val)
    {
        if (currentParFormat.LeftIndentation != val)
        {
            currentParFormat.LeftIndentation = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "li", true, CalcTwips(val)));
        }
    }

    public void SetRightIndentation(float val)
    {
        if (currentParFormat.RightIndentation != val)
        {
            currentParFormat.RightIndentation = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "ri", true, CalcTwips(val)));
        }
    }

    public void SetFormatBold(bool val)
    {
        if (currentFormat.Bold != val)
        {
            currentFormat.Bold = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "b", val ? false : true, 0));
        }
    }

    public void SetFormatItalic(bool val)
    {
        if (currentFormat.Italic != val)
        {
            currentFormat.Italic = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "i", val ? false : true, 0));
        }
    }

    public void SetFormatUnderline(bool val)
    {
        if (currentFormat.Underline != val)
        {
            currentFormat.Underline = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "ul", val ? false : true, 0));
        }
    }

    public void SetFormatColor(Color val)
    {
        if (currentFormat.Color.ToArgb() != val.ToArgb())
        {
            var indColor = colorTable.IndexOf(val);

            if (indColor == -1)
            {
                colorTable.AddColor(val);
                indColor = colorTable.IndexOf(val);
            }

            currentFormat.Color = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "cf", true, indColor));
        }
    }

    public void SetFormatSize(int val)
    {
        if (currentFormat.Size != val)
        {
            currentFormat.Size = val;

            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "fs", true, val * 2));
        }
    }

    public void SetFormatFont(string val)
    {
        if (currentFormat.Font != val)
        {
            var indFont = fontTable.IndexOf(val);

            if (indFont == -1)
            {
                fontTable.AddFont(val);
                indFont = fontTable.IndexOf(val);
            }

            currentFormat.Font = val;
            mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "f", true, indFont));
        }
    }

    public string Text
    {
        get 
        {
            return GetTree().Text;
        }
    }

    public string Rtf
    {
        get
        {
            return GetTree().Rtf;
        }
    }

    public RtfTree Tree
    {
        get
        {
            return GetTree();
        }
    }

    private RtfTree GetTree()
    {
        var tree = new RtfTree();
        tree.RootNode.AppendChild(mainGroup.CloneNode());

        InsertFontTable(tree);
        InsertColorTable(tree);
        InsertGenerator(tree);
        InsertDocSettings(tree);

        tree.MainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "par", false, 0));

        return tree;
    }

    private string GetHexa(byte code)
    {
        var hexa = Convert.ToString(code, 16);

        if (hexa.Length == 1)
        {
            hexa = "0" + hexa;
        }

        return hexa;
    }

    private void InsertFontTable(RtfTree tree)
    {
        var ftGroup = new RtfTreeNode(RtfNodeType.Group);
                
        ftGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "fonttbl", false, 0));

        for(var i=0; i<fontTable.Count; i++)
        {
            var ftFont = new RtfTreeNode(RtfNodeType.Group);
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "f", true, i));
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "fnil", false, 0));
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Text, fontTable[i] + ";", false, 0));

            ftGroup.AppendChild(ftFont);
        }

        tree.MainGroup.InsertChild(5, ftGroup);
    }

    private void InsertColorTable(RtfTree tree)
    {
        var ctGroup = new RtfTreeNode(RtfNodeType.Group);

        ctGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "colortbl", false, 0));

        for (var i = 0; i < colorTable.Count; i++)
        {
            ctGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "red", true, colorTable[i].R));
            ctGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "green", true, colorTable[i].G));
            ctGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "blue", true, colorTable[i].B));
            ctGroup.AppendChild(new RtfTreeNode(RtfNodeType.Text, ";", false, 0));
        }

        tree.MainGroup.InsertChild(6, ctGroup);
    }

    private void InsertGenerator(RtfTree tree)
    {
        var genGroup = new RtfTreeNode(RtfNodeType.Group);

        genGroup.AppendChild(new RtfTreeNode(RtfNodeType.Control, "*", false, 0));
        genGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "generator", false, 0));
        genGroup.AppendChild(new RtfTreeNode(RtfNodeType.Text, "NRtfTree Library 0.3.0;", false, 0));

        tree.MainGroup.InsertChild(7, genGroup);
    }

    private void InsertText(string text)
    {
        var i = 0;
        var code = 0;

        while(i < text.Length)
        {
            code = Char.ConvertToUtf32(text, i);

            if(code >= 32 && code < 128)
            {
                var s = new StringBuilder("");

                while (i < text.Length && code >= 32 && code < 128)
                {
                    s.Append(text[i]);

                    i++;

                    if(i < text.Length)
                        code = Char.ConvertToUtf32(text, i);
                }

                mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Text, s.ToString(), false, 0));
            }
            else
            {
                if (text[i] == '\t')
                {
                    mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "tab", false, 0));
                }
                else if (text[i] == '\n')
                {
                    mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "line", false, 0));
                }
                else
                {
                    if (code <= 255)
                    {
                        var bytes = encoding.GetBytes(new char[] { text[i] });

                        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Control, "'", true, bytes[0]));
                    }
                    else
                    {
                        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "u", true, code));
                        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Text, "?", false, 0));
                    }
                }

                i++;
            }
        }
    }

    private void UpdateFontTable(RtfCharFormat format)
    {
        if (fontTable.IndexOf(format.Font) == -1)
        {
            fontTable.AddFont(format.Font);
        }
    }

    private void UpdateColorTable(RtfCharFormat format)
    {
        if (colorTable.IndexOf(format.Color) == -1)
        {
            colorTable.AddColor(format.Color);
        }
    }

    private void InitializeTree()
    {
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "rtf", true, 1));
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "ansi", false, 0));
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "ansicpg", true, encoding.CodePage));
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "deff", true, 0));
        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "deflang", true, CultureInfo.CurrentCulture.LCID));

        mainGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pard", false, 0));
    }

    private void InsertDocSettings(RtfTree tree)
    {
        var indInicioTexto = tree.MainGroup.ChildNodes.IndexOf("pard");

        tree.MainGroup.InsertChild(indInicioTexto, new RtfTreeNode(RtfNodeType.Keyword, "viewkind", true, 4));
        tree.MainGroup.InsertChild(indInicioTexto++, new RtfTreeNode(RtfNodeType.Keyword, "uc", true, 1));

        tree.MainGroup.InsertChild(indInicioTexto++, new RtfTreeNode(RtfNodeType.Keyword, "margl", true, CalcTwips(docFormat.MarginL)));
        tree.MainGroup.InsertChild(indInicioTexto++, new RtfTreeNode(RtfNodeType.Keyword, "margr", true, CalcTwips(docFormat.MarginR)));
        tree.MainGroup.InsertChild(indInicioTexto++, new RtfTreeNode(RtfNodeType.Keyword, "margt", true, CalcTwips(docFormat.MarginT)));
        tree.MainGroup.InsertChild(indInicioTexto++, new RtfTreeNode(RtfNodeType.Keyword, "margb", true, CalcTwips(docFormat.MarginB)));
    }

    private int CalcTwips(float centimeters)
    {
        return (int)((centimeters * 1440F) / 2.54F);
    }

}