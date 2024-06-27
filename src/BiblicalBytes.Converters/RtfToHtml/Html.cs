using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using BiblicalBytes.Converters.RtfTree;

namespace BiblicalBytes.Converters.RtfToHtml;

public class Html
{
    private StringBuilder builder;
    private Format htmlFormat;
    private List<Format> formatList;
    private int spanCount;
    private int divCount;
    private bool hasHref;
    private RtfColorTable colorTable;
    private RtfFontTable fontTable;

    public Html()
    {
        AutoParagraph = false;
        IgnoreFontNames = false;
        DefaultFontSize = 10;
        DefaultFontName = "Times New Roman";
    }

    public string ConvertRtfToHtml(string rtf) => ConvertCode(rtf);

    public bool AutoParagraph { get; set; }

    public bool IgnoreFontNames { get; set; }

    public bool EscapeHtmlEntities { get; set; }

    public string DefaultFontName { get; set; }

    public int DefaultFontSize { get; set; }
    
    public static string ConvertCode(string rtf)
    {
        var rtfToHtml = new Html();
        return rtfToHtml.Convert(rtf);
    }

    public string Convert(string rtf)
    {
        var rtfTree = new RtfTree.RtfTree();
        rtfTree.LoadRtfText(rtf);
        Console.WriteLine(rtfTree.ToStringEx());
        builder = new StringBuilder();
        htmlFormat = new Format();
        builder.Append("<!DOCTYPE html><html><body>");

        formatList = new List<Format> { new Format() };

        fontTable = rtfTree.GetFontTable();
        colorTable = rtfTree.GetColorTable();

        int beginning;

        for (beginning = 0; beginning < rtfTree.RootNode.FirstChild.ChildNodes.Count; beginning++)
        {
            if (rtfTree.RootNode.FirstChild.ChildNodes[beginning].NodeKey == "pard")
            {
                break;
            }
        }

        TransformChildNodes(rtfTree.RootNode.FirstChild.ChildNodes, beginning);

        ProcessChildNodes(rtfTree.RootNode.FirstChild.ChildNodes, beginning);
        formatList.Last().Reset();
        WriteText(string.Empty);

        if (AutoParagraph)
        {
            string[] partes = builder.ToString().Split(new string[] { "<br /><br />" }, StringSplitOptions.RemoveEmptyEntries);
            builder = new StringBuilder(builder.Length + 7 * partes.Length);

            foreach (var parte in partes)
            {
                builder.Append("<p>");
                builder.Append(parte);
                builder.Append("</p>");
            }
        }
        builder.Append("</body></html>");

        return EscapeHtmlEntities ? HtmlEntities.Encode(builder.ToString()) : builder.ToString();
    }

    private void TransformChildNodes(RtfNodeCollection nodes, int start)
    {
        foreach (RtfTreeNode node in nodes)
        {
            Console.WriteLine(node.NodeKey);
            if (!string.IsNullOrEmpty(node.NodeKey))
            {
                switch (node.NodeType)
                {
                    case RtfNodeType.Group:
                        if (node.HasChildNodes())
                        {
                            TransformChildNodes(node.ChildNodes, 0);
                        }
                        break;
                    case RtfNodeType.Keyword:
                        switch (node.NodeKey)
                        {
                            case "pnlvlblt":
                                node.ParentNode.ParentNode.NodeKey = "ul";
                                node.ParentNode.NodeKey = "";
                                break;

                            case "pnlvlbody":
                                node.ParentNode.ParentNode.NodeKey = "ol";
                                node.ParentNode.NodeKey = "";
                                break;

                            case "pntext":
                                node.ParentNode.ParentNode.NodeKey = "li";
                                break;

                            case "intbl":
                                node.ParentNode.NodeKey = "td";
                                node.ParentNode.ParentNode.NodeKey = "tr";
                                node.ParentNode.ParentNode.ParentNode.NodeKey = "table";
                                break;
                        }
                        break;
                }
            }
        }
    }

    private void ProcessChildNodes(RtfNodeCollection nodes, int start)
    {
        foreach (RtfTreeNode node in nodes.Skip(start))
        {
            if (!string.IsNullOrEmpty(node.NodeKey))
            {
                switch (node.NodeType)
                {
                    case RtfNodeType.Control:
                        ProcessControlNode(node);
                        break;

                    case RtfNodeType.Keyword:
                        ProcessKeywordNode(node);
                        break;

                    case RtfNodeType.Group:
                        ProcessGroupNode(node);
                        break;

                    case RtfNodeType.Text:
                        ProcessTextNode(node);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(node.NodeType), $"Unsupported node type: {node.NodeType}");
                }
            }
        }
    }

    private void ProcessControlNode(RtfTreeNode node)
    {
        if (node.NodeKey == "'")
        {
            Console.WriteLine(node.NodeKey);
            WriteText(Encoding.Default.GetString(new[] { (byte)node.Parameter }));
        }
    }

    private void ProcessKeywordNode(RtfTreeNode node)
    {
        switch (node.NodeKey)
        {
            case "pard":
                break;
            case "pntext":
                formatList.Last().IsLi = true;
                break;
            case "f":
                if (node.Parameter < fontTable.Count)
                    formatList.Last().FontName = fontTable[node.Parameter];
                break;
            case "cf":
                if (node.Parameter < colorTable.Count)
                    formatList.Last().ForeColor = colorTable[node.Parameter];
                break;
            case "highlight":
                if (node.Parameter < colorTable.Count)
                    formatList.Last().BackColor = colorTable[node.Parameter];
                Console.WriteLine("Highlight color set");
                break;
            case "fs":
                formatList.Last().FontSize = node.Parameter;
                break;
            case "b":
            case "i":
            case "strike":
            case "em":
                ProcessTextFormattingKeyword(node);
                break;
            case "ul":
                formatList.Last().Underline = true;
                break;
            case "ulnone":
                formatList.Last().Underline = false;
                break;
            case "super":
                formatList.Last().Superscript = true;
                formatList.Last().Subscript = false;
                break;
            case "sub":
                formatList.Last().Subscript = true;
                formatList.Last().Superscript = false;
                break;
            case "nosupersub":
                formatList.Last().Superscript = formatList.Last().Subscript = false;
                break;
            case "qc":
            case "qr":
                ProcessAlignmentKeyword(node);
                break;
            case "li":
                formatList.Last().Margin = node.Parameter;
                break;
            case "line":
            case "par":
                builder.Append("<br>");
                break;
            case "fonttbl":
            case "colortbl":
                ClearNodeKeys(node.ParentNode.ChildNodes);
                break;
            default:
                break;
        }
    }

    private void ProcessGroupNode(RtfTreeNode node)
    {
        if (node.HasChildNodes())
        {
            ProcessGroupTags(node);
            formatList.Add(new Format());
            ProcessChildNodes(node.ChildNodes, 0);
            CloseGroupTags(node);
            formatList.RemoveAt(formatList.Count - 1);
            UpdateHtmlFormatFromLast();
        }
    }

    private void ProcessTextNode(RtfTreeNode node)
    {
        if (node.NodeKey.Contains("HYPERLINK"))
        {
            var href = node.NodeKey.Replace("HYPERLINK", "<a href=").Replace("\\", "") + ">";
            formatList.Last().HasHref = true;
            builder.Append(href);
        }
        else
        {
            Console.WriteLine(node.NodeKey);
            WriteText(node.NodeKey, false);
        }
        formatList.Last().IsOpen = true;
    }

    private void ProcessTextFormattingKeyword(RtfTreeNode node)
    {
        switch (node.NodeKey)
        {
            case "b":
                formatList.Last().Bold = node.NodeKey == "b";
                break;
            case "i":
                formatList.Last().Italic = node.NodeKey == "i";
                break;
            case "strike":
                formatList.Last().Strike = node.NodeKey == "strike";
                break;
            case "em":
                formatList.Last().Italic = node.NodeKey == "em";
                break;
        }
    }

    private void ProcessAlignmentKeyword(RtfTreeNode node)
    {
        switch (node.NodeKey)
        {
            case "qc":
                formatList.Last().Alignment = HorizontalAlignment.Center;
                break;
            case "qr":
                formatList.Last().Alignment = HorizontalAlignment.Right;
                break;
        }
    }

    private void ClearNodeKeys(RtfNodeCollection nodes)
    {
        foreach (RtfTreeNode node in nodes)
        {
            node.NodeKey = string.Empty;
        }
    }

    private void ProcessGroupTags(RtfTreeNode node)
    {
        if (node.NodeKey == "ul")
        {
            builder.Append("<ul>");
        }
        else if (node.NodeKey == "ol")
        {
            builder.Append("<ol>");
        }
        else if (node.NodeKey == "li")
        {
            builder.Append("<li>");
        }
        else if (node.NodeKey == "table")
        {
            builder.Append("<table>");
        }
        else if (node.NodeKey == "tr")
        {
            builder.Append("<tr>");
        }
        else if (node.NodeKey == "td")
        {
            builder.Append("<td>");
        }
    }

    private void CloseGroupTags(RtfTreeNode node)
    {
        if (node.NodeKey == "ul")
        {
            builder.Append("</ul>");
        }
        else if (node.NodeKey == "ol")
        {
            builder.Append("</ol>");
        }
        else if (node.NodeKey == "li")
        {
            builder.Append("</li>");
        }
        else if (node.NodeKey == "table")
        {
            builder.Append("</table>");
        }
        else if (node.NodeKey == "tr")
        {
            builder.Append("</tr>");
        }
        else if (node.NodeKey == "td")
        {
            builder.Append("</td>");
        }
    }

    private void UpdateHtmlFormatFromLast()
    {
        htmlFormat = formatList.Last();
    }

    private void UpdateHtmlFormatToLast()
    {
        formatList.Last().CompareFontFormat(htmlFormat);
    }

    private void WriteText(string text, bool update = true)
    {

        if (update)

        {
            if (builder.Length > 0)
            {
                if (formatList.Last().Bold == true)
                {
                    builder.Append("</strong>");
                    htmlFormat.Bold = false;
                }
                if (formatList.Last().Italic == true)
                {
                    builder.Append("</em>");
                    htmlFormat.Italic = false;
                }

                if (formatList.Last().Strike == true)
                {
                    builder.Append("</strike>");
                    htmlFormat.Strike = false;
                }
                if (formatList.Last().Underline == true)
                {
                    builder.Append("</u>");
                    htmlFormat.Underline = false;
                }
                if (formatList.Last().Subscript == true)
                {
                    builder.Append("</sub>");
                    htmlFormat.Subscript = false;
                }
                if (formatList.Last().Superscript == true)
                {
                    builder.Append("</sup>");
                    htmlFormat.Superscript = false;
                }
                if ((formatList.Last().CompareFontFormat(htmlFormat) == false || text.Contains("close")) && formatList.Last().SpanIsOpen)      
                {

                    if (spanCount > 0)
                    {
                        builder.Append("</span>");

                        spanCount--;
                    }
                    if (divCount > 0)
                    {

                        builder.Append("</div>");
                        divCount--;

                    }
                    htmlFormat.Reset();
                }
            }
        }
        if (!text.Contains("close"))
        {
            if (formatList.Last().CompareFontFormat(htmlFormat) == false)      
            {
                var estilo = string.Empty;

                if (!IgnoreFontNames && !string.IsNullOrEmpty(formatList.Last().FontName) &&
                    string.Compare(formatList.Last().FontName, DefaultFontName, StringComparison.OrdinalIgnoreCase) != 0)
                    estilo += $"font-family:{"\'" + formatList.Last().FontName.Trim() + "\'"};";
                
                if (formatList.Last().FontSize > 0 && formatList.Last().FontSize / 2 != DefaultFontSize)
                    estilo += $"font-size:{formatList.Last().FontSize / 2}pt;";
                
                if (formatList.Last().Margin != htmlFormat.Margin)
                    estilo += $"margin-left:{formatList.Last().Margin / 15}px;";
                
                if (formatList.Last().Alignment != HorizontalAlignment.Left)
                    estilo += $"text-align:{formatList.Last().Alignment.ToString().ToLower()};";
                
                if (CompareColor(formatList.Last().ForeColor, htmlFormat.ForeColor) == false)
                    estilo += $"color:{ColorTranslator.ToHtml(formatList.Last().ForeColor)};";
                
                if (formatList.Last().BackColor != System.Drawing.Color.White)
                    estilo += $"background-color:{ColorTranslator.ToHtml(formatList.Last().BackColor)};";

                htmlFormat.FontName = formatList.Last().FontName;
                htmlFormat.FontSize = formatList.Last().FontSize;
                htmlFormat.ForeColor = formatList.Last().ForeColor;
                htmlFormat.BackColor = formatList.Last().BackColor;
                htmlFormat.Margin = formatList.Last().Margin;
                htmlFormat.Alignment = formatList.Last().Alignment;

                if (!string.IsNullOrEmpty(estilo))
                {
                    if (estilo.Contains("text-align") && formatList.Last().Alignment != HorizontalAlignment.Left)
                    {
                        Console.Write(estilo);
                        formatList.Last().SpanIsOpen = true;
                        builder.AppendFormat("<div style=\"{0}\">", estilo);
                        divCount++;
                    }
                    else
                    {
                        Console.Write(estilo);
                        formatList.Last().SpanIsOpen = true;

                        builder.AppendFormat("<span style=\"{0}\">", estilo);
                        spanCount++;
                    }
                }
            }
            if (formatList.Last().Superscript && htmlFormat.Superscript == false)
            {
                builder.Append("<sup>");
                htmlFormat.Superscript = true;
            }
            if (formatList.Last().Subscript && htmlFormat.Subscript == false)
            {
                builder.Append("<sub>");
                htmlFormat.Subscript = true;
            }
            if (formatList.Last().Strike && htmlFormat.Strike == false)
            {
                builder.Append("<strike>");
                htmlFormat.Strike = true;
            }

            if (formatList.Last().Underline && htmlFormat.Underline == false)
            {
                builder.Append("<u>");
                htmlFormat.Underline = true;
            }
            if (formatList.Last().Italic && htmlFormat.Italic == false)
            {
                builder.Append("<em>");
                htmlFormat.Italic = true;
            }
            if (formatList.Last().Bold && htmlFormat.Bold == false)
            {
                builder.Append("<strong>");
                htmlFormat.Bold = true;
            }

        }
        if (text == "close")
        {
            text = "";
            if (hasHref)
            {
                builder.Append("</a>");
            }
        }
        builder.Append(text.Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;"));
    }

    private static bool CompareColor(System.Drawing.Color a, System.Drawing.Color b)
    {
        return a.R == b.R && a.G == b.G && a.B == b.B;
    }
}

public class Format
{
    public bool Italic;
    public bool Bold;
    public bool Subscript;
    public bool Strike;
    public bool Underline;
    public bool IsLi;
    public bool Superscript;
    public bool HasHref;
    public string FontName;
    public int FontSize;
    public System.Drawing.Color ForeColor;
    public System.Drawing.Color BackColor;
    public int Margin;
    public bool SpanIsOpen;
    public bool IsOpen;
    public HorizontalAlignment Alignment;

    public Format()
    {
        Reset();
    }

    public bool CompareFontFormat(Format format)
    {
        return string.Compare(FontName, format.FontName, true) == 0 &&
               FontSize == format.FontSize &&
               ForeColor == format.ForeColor &&
               Strike == format.Strike &&
               BackColor == format.BackColor &&
               Margin == format.Margin &&
               Alignment == format.Alignment;
    }

    public void Reset()
    {
        FontName = string.Empty;
        FontSize = 0;
        Bold = false;
        Subscript = false;
        Italic = false;
        Strike = false;
        BackColor = System.Drawing.Color.White;
        Underline = false;
        Margin = 0;
        SpanIsOpen = false;
        Alignment = HorizontalAlignment.Left;
        IsOpen = false;
    }
}

public enum HorizontalAlignment
{
    Left,
    Right,
    Center
}