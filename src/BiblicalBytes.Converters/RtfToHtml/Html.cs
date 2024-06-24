using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using BiblicalBytes.Converters.RtfTree;

namespace BiblicalBytes.Converters.RtfToHtml;

internal class Html
{
    public string ConvertRtfToHtml(string rtf)
    {
        //string htmlWithoutStrangerTags = this.swapHtmlStrangerTags(html)
        return ConvertCode(rtf);

    }
    private StringBuilder builder;
    private Format htmlFormat;
    private List<Format> formatList;
    private int spanCount = 0;
    private int divCount = 0;
    private bool hasHref = false;
    private RtfColorTable colorTable;
    private RtfFontTable fontTable;
    private bool autoParagraph;
    private bool ignoreFontNames;
    private bool escapeHtmlEntities;
    private string defaultFontName;
    private int defaultFontSize;

    public Html()
    {
        AutoParagraph = false;
        IgnoreFontNames = false;
        DefaultFontSize = 10;
        //EscapeHtmlEntities = true;
        DefaultFontName = "Times New Roman";
    }
    
    public bool AutoParagraph
    {
        get
        {
            return autoParagraph;
        }
        set
        {
            autoParagraph = value;
        }
    }
    public bool IgnoreFontNames
    {
        get
        {
            return ignoreFontNames;
        }
        set
        {
            ignoreFontNames = value;
        }
    }
    public bool EscapeHtmlEntities
    {
        get
        {
            return escapeHtmlEntities;
        }
        set
        {
            escapeHtmlEntities = value;
        }
    }
    public string DefaultFontName
    {
        get
        {
            return defaultFontName;
        }
        set
        {
            defaultFontName = value;
        }
    }
    public int DefaultFontSize
    {
        get
        {
            return defaultFontSize;
        }
        set
        {
            defaultFontSize = value;
        }
    }


    /// <summary>
    /// Convierte una cadena de código RTF a formato HTML
    /// </summary>
    public static string ConvertCode(string rtf)
    {
        var rtfToHtml = new Html();
        return rtfToHtml.Convert(rtf);
    }

    /// <summary>
    /// Convierte una cadena de código RTF a formato HTML
    /// </summary>
    public string Convert(string rtf)
    {
        //Console.WriteLine(rtf);
        //Generar arbol DOM
        var rtfTree = new RtfTree.RtfTree();
        rtfTree.LoadRtfText(rtf);
        Console.WriteLine(rtfTree.ToStringEx());
        //Inicializar variables empleadas
        builder = new StringBuilder();
        htmlFormat = new Format();
        this.builder.Append("<!DOCTYPE html><html><body> ");
        formatList =
        [
            new Format()
        ];
        try
        {
            fontTable = rtfTree.GetFontTable();
        }
        catch
        {

        }
        try
        {
            colorTable = rtfTree.GetColorTable();
        }
        catch
        {

        }
        int inicio;
        for (inicio = 0; inicio < rtfTree.RootNode.FirstChild.ChildNodes.Count; inicio++)
        {

            if (rtfTree.RootNode.FirstChild.ChildNodes[inicio].NodeKey == "pard")
            {
                break;
            }
        }
        //Procesar todos los nodos visibles
        TransformChildNodes(rtfTree.RootNode.FirstChild.ChildNodes, inicio);

        ProcessChildNodes(rtfTree.RootNode.FirstChild.ChildNodes, inicio);
        //Cerrar etiquetas pendientes
        formatList.Last().Reset();
        WriteText(string.Empty);
        var repairList = new Regex("<span [^>]*>·</span><span style=\"([^\"]*)\">(.*?)<br\\s+/><" + "/span>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline |
            RegexOptions.CultureInvariant);

        //foreach (Match match in repairList.Matches(_builder.ToString()))
        //{
        //    _builder.Replace(match.Value, string.Format("<li style=\"{0}\">{1}</li>", match.Groups[1].Value, match.Groups[2].Value));
        //}

        //Regex repairUl = new Regex("(?<!</li>)<li", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        //foreach (Match match in repairUl.Matches(_builder.ToString()))
        //{
        //    _builder.Insert(match.Index, "<ul>");
        //}

        //repairUl = new Regex("/li>(?!<li)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        //foreach (Match match in repairUl.Matches(_builder.ToString()))
        //{
        //    _builder.Insert(match.Index + match.Length, "</ul>");
        //}


        if (AutoParagraph)
        {
            string[] partes = builder.ToString().Split(["<br /><br />"], StringSplitOptions.RemoveEmptyEntries);
            builder = new StringBuilder(builder.Length + 7 * partes.Length);

            foreach (var parte in partes)
            {
                builder.Append("<p>");
                builder.Append(parte);
                builder.Append("</p>");
            }
        }
        this.builder.Append("</body></html>");

        //Console.WriteLine(_builder.ToString());
        return EscapeHtmlEntities ? HtmlEntities.Encode(builder.ToString()) : builder.ToString();

    }


    private void TransformChildNodes(RtfNodeCollection nodos, int inicio)
    {
        //for (int i =0;i<nodos.Count-1;i++)
        foreach (RtfTreeNode nod in nodos)
        {
            //if(nodo.)
            Console.WriteLine(nod.NodeKey);
            if (nod.NodeKey != "")
            {
                switch (nod.NodeType)
                {
                    case RtfNodeType.Group:
                        {
                            //Procesar nodos hijo, si los tiene
                            if (nod.HasChildNodes())
                            {

                                TransformChildNodes(nod.ChildNodes, 0);


                            }
                            break;
                        }
                    case RtfNodeType.Keyword:
                        {
                            //Console.WriteLine(nod.NodeKey);
                            if (nod.NodeKey == "pnlvlblt")
                            {

                                nod.ParentNode.ParentNode.NodeKey = "ul";
                                foreach (RtfTreeNode node in nod.ParentNode.ParentNode.ChildNodes)
                                {

                                    if (node.NodeType == RtfNodeType.Keyword)
                                    {

                                        //node.ParentNode.AppendChild(node);
                                    }
                                }
                                //Console.WriteLine(nod.ParentNode.NodeKey);
                                nod.ParentNode.NodeKey = "";

                            }

                            if (nod.NodeKey == "pnlvlbody")
                            {

                                nod.ParentNode.ParentNode.NodeKey = "ol";
                                foreach (RtfTreeNode node in nod.ParentNode.ParentNode.ChildNodes)
                                {

                                    if (node.NodeType == RtfNodeType.Keyword)
                                    {
                                        //node.ParentNode.ParentNode.AppendChild(node);
                                    }
                                }
                                nod.ParentNode.NodeKey = "";

                            }
                            if (nod.NodeKey == "pntext")
                            {
                                nod.ParentNode.ParentNode.NodeKey = "li";
                            }
                            if (nod.NodeKey == "intbl")
                            {
                                nod.ParentNode.NodeKey = "td";
                                nod.ParentNode.ParentNode.NodeKey = "tr";
                                nod.ParentNode.ParentNode.ParentNode.NodeKey = "table";

                            }
                            break;
                        }
                }
            }
        }
    }


    private void ProcessChildNodes(RtfNodeCollection nodos, int inicio)
    {
        foreach (RtfTreeNode nodo in nodos)
        {
            //if(nodo.)
            // Console.WriteLine(nodo.NodeKey);
            if (nodo.NodeKey != "")
            {
                switch (nodo.NodeType)
                {

                    case RtfNodeType.Control:

                        if (nodo.NodeKey == "'")
                        {
                            Console.WriteLine(nodo.NodeKey);
                            WriteText(Encoding.Default.GetString(new[] { (byte)nodo.Parameter }));
                        }
                        break;

                    case RtfNodeType.Keyword:

                        switch (nodo.NodeKey)
                        {
                            case "pard":
                                //Reinicio de formato
                                //_formatList.Last().Reset();
                                break;
                            case "pntext":
                                {
                                    formatList.Last().IsLi = true;
                                    break;
                                }
                            case "f": //Tipo de fuente                                
                                if (nodo.Parameter < fontTable.Count)
                                    formatList.Last().FontName = fontTable[nodo.Parameter];
                                break;

                            case "cf": //Color de fuente
                                if (nodo.Parameter < colorTable.Count)
                                    formatList.Last().ForeColor = colorTable[nodo.Parameter];
                                break;

                            case "highlight": //Color de fondo
                                if (nodo.Parameter < colorTable.Count)
                                    formatList.Last().BackColor = colorTable[nodo.Parameter];
                                Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                                break;

                            case "fs": //Tamaño de fuente
                                formatList.Last().FontSize = nodo.Parameter;
                                break;

                            case "b": //Negrita
                                formatList.Last().Bold = !nodo.HasParameter || nodo.Parameter == 1;
                                break;

                            case "i": //Cursiva
                                formatList.Last().Italic = !nodo.HasParameter || nodo.Parameter == 1;
                                break;
                            case "strike": //Cursiva
                                formatList.Last().Strike = !nodo.HasParameter || nodo.Parameter == 1;
                                break;
                            case "em": //Cursiva
                                formatList.Last().Italic = !nodo.HasParameter || nodo.Parameter == 1;
                                break;

                            case "ul": //Subrayado ON
                                formatList.Last().Underline = true;
                                break;

                            case "ulnone": //Subrayado OFF
                                formatList.Last().Underline = false;
                                break;

                            case "super": //Superscript
                                formatList.Last().Superscript = true;
                                formatList.Last().Subscript = false;
                                break;
                            case "fonttbl":
                            case "colortbl":
                                {
                                    var i = nodo.ParentNode.ChildNodes.Count - 1;
                                    foreach (RtfTreeNode node in nodo.ParentNode.ChildNodes)
                                    {
                                        node.NodeKey = "";
                                        Console.WriteLine(node.NodeKey);
                                        i--;
                                    }
                                    break;
                                }
                            case "sub": //Subindice
                                formatList.Last().Subscript = true;
                                formatList.Last().Superscript = false;
                                break;

                            case "nosupersub":
                                formatList.Last().Superscript = formatList.Last().Subscript = false;
                                break;

                            case "qc": //Alineacion centrada
                                if (nodo.ParentNode.NodeKey != "td")
                                    formatList.Last().Alignment = HorizontalAlignment.Center;
                                break;

                            case "qr": //Alineacion derecha
                                if (nodo.ParentNode.NodeKey != "td")
                                    formatList.Last().Alignment = HorizontalAlignment.Right;
                                break;

                            case "li": //tabulacion
                                formatList.Last().Margin = nodo.Parameter;
                                break;

                            case "line":
                            case "par": //Nueva línea
                                builder.Append("<br>");
                                //_formatList.Last().Reset();

                                break;
                            default:
                                break;
                        }

                        break;

                    case RtfNodeType.Group:
                        {

                            //Procesar nodos hijo, si los tiene
                            if (nodo.HasChildNodes())
                            {
                                if (nodo.NodeKey == "ul")
                                {
                                    builder.Append("<ul>");
                                }
                                if (nodo.NodeKey == "ol")
                                {
                                    builder.Append("<ol>");
                                }
                                if (nodo.NodeKey == "td")
                                {
                                    builder.Append("<td>");
                                }
                                if (nodo.NodeKey == "table")
                                {
                                    builder.Append("<table>");
                                    builder.Append("<tbody>");

                                }
                                if (nodo.NodeKey == "tr")
                                {
                                    builder.Append("<tr>");
                                }
                                if (nodo.NodeKey == "li")
                                {
                                    builder.Append("<li>");
                                }
                                else
                                {
                                    if (formatList.Last().IsOpen)
                                        WriteText("", false);
                                    else
                                        WriteText("", true);
                                }
                                formatList.Add(new Format());

                                ProcessChildNodes(nodo.ChildNodes, 0);

                                if (nodo.NodeKey == "ul")
                                {
                                    builder.Append("</ul>");

                                }
                                if (nodo.NodeKey == "ol")
                                {
                                    builder.Append("</ol>");

                                }
                                if (nodo.NodeKey == "table")
                                {
                                    builder.Append("</tbody>");
                                    builder.Append("</table>");

                                }
                                if (nodo.NodeKey == "td")
                                {
                                    builder.Append("</td>");

                                }
                                if (nodo.NodeKey == "li")
                                {
                                    builder.Append("</li>");
                                }
                                if (nodo.NodeKey == "tr")
                                {
                                    builder.Append("</tr>");
                                }
                                else
                                {
                                    WriteText("close");
                                    hasHref = false;
                                    if (formatList.Last().HasHref)
                                    {
                                        hasHref = true;
                                    }
                                }
                                formatList.RemoveAt(formatList.Count - 1);
                                htmlFormat.FontName = formatList.Last().FontName;
                                htmlFormat.FontSize = formatList.Last().FontSize;
                                htmlFormat.ForeColor = formatList.Last().ForeColor;
                                htmlFormat.BackColor = formatList.Last().BackColor;
                                htmlFormat.Margin = formatList.Last().Margin;
                                htmlFormat.Alignment = formatList.Last().Alignment;


                            }
                            break;
                        }

                    case RtfNodeType.Text:
                        {
                            var href = "";
                            if (nodo.NodeKey.Contains("HYPERLINK"))
                            {
                                href = nodo.NodeKey.Replace("HYPERLINK", "<a href=").Replace("\\", "") + ">";
                                formatList.Last().HasHref = true;
                                builder.Append(href);
                                formatList.Last().IsOpen = true;
                            }
                            else
                            {
                                Console.WriteLine(nodo.NodeKey);
                                WriteText(nodo.NodeKey, false);
                                formatList.Last().IsOpen = true;
                            }
                        }
                        break;

                    default:

                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    /// <summary>
    /// Función encargada de añadir texto con el formato actual al código html resultado
    /// </summary>
    private void WriteText(string text, bool update = true)
    {

        if (update)

        {
            if (builder.Length > 0)
            {
                //Cerrar etiquetas
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
                if ((formatList.Last().CompareFontFormat(htmlFormat) == false || text.Contains("close")) && formatList.Last().SpanIsOpen) //El formato de fuente ha cambiado
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
                    //Reiniciar propiedades
                    htmlFormat.Reset();
                }
            }
        }
        if (!text.Contains("close"))
        {
            //Abrir etiquetas necesarias para representar el formato actual
            if (formatList.Last().CompareFontFormat(htmlFormat) == false) //El formato de fuente ha cambiado
            {
                var estilo = string.Empty;

                if (!IgnoreFontNames && !string.IsNullOrEmpty(formatList.Last().FontName) &&
                    string.Compare(formatList.Last().FontName, DefaultFontName, true) != 0)
                    estilo += string.Format("font-family:{0};", "\'" + formatList.Last().FontName.Trim() + "\'");
                if (formatList.Last().FontSize > 0 && formatList.Last().FontSize / 2 != DefaultFontSize)
                    estilo += string.Format("font-size:{0}pt;", formatList.Last().FontSize / 2);
                if (formatList.Last().Margin != htmlFormat.Margin)
                    estilo += string.Format("margin-left:{0}px;", formatList.Last().Margin / 15);
                if (formatList.Last().Alignment != HorizontalAlignment.Left)
                    estilo += string.Format("text-align:{0};", formatList.Last().Alignment.ToString().ToLower());
                if (CompareColor(formatList.Last().ForeColor, htmlFormat.ForeColor) == false)
                    estilo += string.Format("color:{0};", ColorTranslator.ToHtml(formatList.Last().ForeColor));
                if (formatList.Last().BackColor != System.Drawing.Color.White)
                    estilo += string.Format("background-color:{0};", ColorTranslator.ToHtml(formatList.Last().BackColor));

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
        //_htmlFormat.Reset();

    }

    private static bool CompareColor(System.Drawing.Color a, System.Drawing.Color b)
    {
        return a.R == b.R && a.G == b.G && a.B == b.B;
    }


    private class Format
    {
        public bool Italic;
        public bool Bold;
        public bool Subscript;
        public bool Strike;
        public bool Underline;
        public bool IsLi = false;
        public bool Superscript;
        public bool HasHref = false;
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

        /// <summary>
        /// Compara las propiedades FontName, FontSize, Margin, ForeColor, BackColor y Alignment con otro
        /// objeto de esta clase
        /// </summary>
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

    private enum HorizontalAlignment
    {
        Left,
        Right,
        Center
    }

}