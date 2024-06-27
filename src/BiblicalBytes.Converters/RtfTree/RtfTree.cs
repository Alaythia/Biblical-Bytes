using System.Drawing;
using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfTree
{
    private RtfTreeNode rootNode;
    private TextReader rtf;
    private RtfLex lex;
    private RtfToken tok;
    private int level;
    private bool mergeSpecialCharacters;

    public RtfTree()
    {
        rootNode = new RtfTreeNode(RtfNodeType.Root,"ROOT",false,0);

        rootNode.Tree = this;

        mergeSpecialCharacters = false;

    }

    public RtfTree CloneTree()
    {
        var clon = new RtfTree();

        clon.rootNode = rootNode.CloneNode();

        return clon;
    }

    public int LoadRtfFile(string path)
    {
        var res = 0;

        rtf = new StreamReader(path);

        lex = new RtfLex(rtf);

        res = ParseRtfTree();

        rtf.Close();

        return res;
    }

    public int LoadRtfText(string text)
    {
        var res = 0;

        rtf = new StringReader(text);

        lex = new RtfLex(rtf);

        res = ParseRtfTree();

        rtf.Close();

        return res;
    }

    public void SaveRtf(string filePath)
    { 
        var sw = new StreamWriter(filePath);

        sw.Write(RootNode.Rtf);

        sw.Flush();
        sw.Close();
    }

    public override string ToString()
    {
        var res = "";

        res = ToStringInm(rootNode, 0, false);

        return res;
    }

    public string ToStringEx()
    {
        var res = "";

        res = ToStringInm(rootNode, 0, true);

        return res;
    }

    public RtfFontTable GetFontTable()
    {
        var tablaFuentes = new RtfFontTable();

        var root = rootNode;

        var nprin = root.FirstChild;

        if(nprin == null)
            return tablaFuentes;

        var enc = false;
        var i = 0;
        var ntf = new RtfTreeNode();       

        while (!enc && i < nprin.ChildNodes.Count)
        {
            if (nprin.ChildNodes[i].NodeType == RtfNodeType.Group &&
                nprin.ChildNodes[i].FirstChild.NodeKey == "fonttbl")
            {
                enc = true;
                ntf = nprin.ChildNodes[i];
            }

            i++;
        }

        for (var j = 1; j < ntf.ChildNodes.Count; j++)
        {
            var fuente = ntf.ChildNodes[j];

            var indiceFuente = -1;
            string nombreFuente = null;

            foreach (RtfTreeNode nodo in fuente.ChildNodes)
            {
                if (nodo.NodeKey == "f")
                    indiceFuente = nodo.Parameter;

                if (nodo.NodeType == RtfNodeType.Text)
                    nombreFuente = nodo.NodeKey.Substring(0, nodo.NodeKey.Length - 1);
            }

            tablaFuentes.AddFont(indiceFuente, nombreFuente);
        }

        return tablaFuentes;
    }

    public RtfColorTable GetColorTable()
    {
        var tablaColores = new RtfColorTable();

        var root = rootNode;

        var nprin = root.FirstChild;

        if(nprin == null)
            return tablaColores;

        var enc = false;
        var i = 0;
        var ntc = new RtfTreeNode();       

        while (!enc && i < nprin.ChildNodes.Count)
        {
            if (nprin.ChildNodes[i].NodeType == RtfNodeType.Group &&
                nprin.ChildNodes[i].FirstChild.NodeKey == "colortbl")
            {
                enc = true;
                ntc = nprin.ChildNodes[i];
            }

            i++;
        }

        var rojo = 0;
        var verde = 0;
        var azul = 0;

        for (var j = 1; j < ntc.ChildNodes.Count; j++)
        {
            var nodo = ntc.ChildNodes[j];

            if (nodo.NodeType == RtfNodeType.Text && nodo.NodeKey.Trim() == ";")
            {
                tablaColores.AddColor(Color.FromArgb(rojo, verde, azul));

                rojo = 0;
                verde = 0;
                azul = 0;
            }
            else if (nodo.NodeType == RtfNodeType.Keyword)
            {
                switch (nodo.NodeKey)
                {
                    case "red":
                        rojo = nodo.Parameter;
                        break;
                    case "green":
                        verde = nodo.Parameter;
                        break;
                    case "blue":
                        azul = nodo.Parameter;
                        break;
                }
            }
        }

        return tablaColores;
    }

    public RtfStyleSheetTable GetStyleSheetTable()
    {
        var sstable = new RtfStyleSheetTable();

        var sst = MainGroup.SelectSingleGroup("stylesheet");

        var styles = sst.ChildNodes;

        for (var i = 1; i < styles.Count; i++)
        {
            var style = styles[i];

            var rtfss = ParseStyleSheet(style);

            sstable.AddStyleSheet(rtfss.Index, rtfss);
        }

        return sstable;
    }

    public InfoGroup? GetInfoGroup()
    {
        InfoGroup? info = null;

        var infoNode = RootNode.SelectSingleNode("info");

        if (infoNode != null)
        {
            RtfTreeNode auxnode = null;

            info = new InfoGroup();

            if ((auxnode = rootNode.SelectSingleNode("title")) != null)
                info.Title = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("subject")) != null)
                info.Subject = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("author")) != null)
                info.Author = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("manager")) != null)
                info.Manager = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("company")) != null)
                info.Company = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("operator")) != null)
                info.Operator = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("category")) != null)
                info.Category = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("keywords")) != null)
                info.Keywords = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("comment")) != null)
                info.Comment = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("doccomm")) != null)
                info.DocComment = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("hlinkbase")) != null)
                info.HlinkBase = auxnode.NextSibling.NodeKey;

            if ((auxnode = rootNode.SelectSingleNode("version")) != null)
                info.Version = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("vern")) != null)
                info.InternalVersion = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("edmins")) != null)
                info.EditingTime = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("nofpages")) != null)
                info.NumberOfPages = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("nofchars")) != null)
                info.NumberOfChars = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("nofwords")) != null)
                info.NumberOfWords = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("id")) != null)
                info.Id = auxnode.Parameter;

            if ((auxnode = rootNode.SelectSingleNode("creatim")) != null)
                info.CreationTime = ParseDateTime(auxnode.ParentNode);

            if ((auxnode = rootNode.SelectSingleNode("revtim")) != null)
                info.RevisionTime = ParseDateTime(auxnode.ParentNode);
                    
            if ((auxnode = rootNode.SelectSingleNode("printim")) != null)
                info.LastPrintTime = ParseDateTime(auxnode.ParentNode);

            if ((auxnode = rootNode.SelectSingleNode("buptim")) != null)
                info.BackupTime = ParseDateTime(auxnode.ParentNode);
        }

        return info;
    }

    public Encoding GetEncoding()
    {
        var encoding = Encoding.Default;

        var cpNode = RootNode.SelectSingleNode("ansicpg");

        if (cpNode != null)
        {
            encoding = Encoding.GetEncoding(cpNode.Parameter);
        }

        return encoding;
    }

    private int ParseRtfTree()
    {
        var res = 0;

        var encoding = Encoding.Default;

        var curNode = rootNode;

        RtfTreeNode newNode = null;

        tok = lex.NextToken();

        while (tok.Type != RtfTokenType.Eof)
        {
            switch (tok.Type)
            {
                case RtfTokenType.GroupStart:
                    newNode = new RtfTreeNode(RtfNodeType.Group,"GROUP",false,0);
                    curNode.AppendChild(newNode);
                    curNode = newNode;
                    level++;
                    break;
                case RtfTokenType.GroupEnd:
                    curNode = curNode.ParentNode;
                    level--;
                    break;
                case RtfTokenType.Keyword:
                case RtfTokenType.Control:
                case RtfTokenType.Text:
                    if (mergeSpecialCharacters)
                    {
                        var isText = tok.Type == RtfTokenType.Text || (tok.Type == RtfTokenType.Control && tok.Key == "'");
                        if (curNode.LastChild != null && (curNode.LastChild.NodeType == RtfNodeType.Text && isText))
                        {
                            if (tok.Type == RtfTokenType.Text)
                            {
                                curNode.LastChild.NodeKey += tok.Key;
                                break;
                            }
                            if (tok.Type == RtfTokenType.Control && tok.Key == "'")
                            {
                                curNode.LastChild.NodeKey += DecodeControlChar(tok.Parameter, encoding);
                                break;
                            }
                        }
                        else
                        {
                            if (tok.Type == RtfTokenType.Control && tok.Key == "'")
                            {
                                newNode = new RtfTreeNode(RtfNodeType.Text, DecodeControlChar(tok.Parameter, encoding), false, 0);
                                curNode.AppendChild(newNode);
                                break;
                            }
                        }
                    }

                    newNode = new RtfTreeNode(tok);
                    curNode.AppendChild(newNode);

                    if (mergeSpecialCharacters)
                    {
                        if (level == 1 && newNode.NodeType == RtfNodeType.Keyword && newNode.NodeKey == "ansicpg")
                        {
                            encoding = Encoding.GetEncoding(newNode.Parameter);
                        }
                    }

                    break;
                default:
                    res = -1;
                    break;
            }

            tok = lex.NextToken();
        }

        if (level != 0)
        {
            res = -1;
        }

        return res;
    }

    private string DecodeControlChar(int code, Encoding enc)
    {
        return enc.GetString(new byte[] {(byte)code});                
    }

    private string ToStringInm(RtfTreeNode curNode, int level, bool showNodeTypes)
    {
        var res = new StringBuilder();

        var children = curNode.ChildNodes;

        for (var i = 0; i < level; i++)
            res.Append("  ");

        if (curNode.NodeType == RtfNodeType.Root)
            res.Append("ROOT\r\n");
        else if (curNode.NodeType == RtfNodeType.Group)
            res.Append("GROUP\r\n");
        else
        {
            if (showNodeTypes)
            {
                res.Append(curNode.NodeType);
                res.Append(": ");
            }

            res.Append(curNode.NodeKey);

            if (curNode.HasParameter)
            {
                res.Append(" ");
                res.Append(Convert.ToString(curNode.Parameter));
            }

            res.Append("\r\n");
        }

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                res.Append(ToStringInm(node, level + 1, showNodeTypes));
            }
        }

        return res.ToString();
    }

    private static DateTime ParseDateTime(RtfTreeNode group)
    {
        DateTime dt;

        int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0;

        foreach (RtfTreeNode node in group.ChildNodes)
        {
            switch (node.NodeKey)
            {
                case "yr":
                    year = node.Parameter;
                    break;
                case "mo":
                    month = node.Parameter;
                    break;
                case "dy":
                    day = node.Parameter;
                    break;
                case "hr":
                    hour = node.Parameter;
                    break;
                case "min":
                    min = node.Parameter;
                    break;
                case "sec":
                    sec = node.Parameter;
                    break;
            }
        }

        dt = new DateTime(year, month, day, hour, min, sec);

        return dt;
    }

    private string ConvertToText()
    {
        var res = new StringBuilder("");

        var pardNode =
            MainGroup.SelectSingleChildNode("pard");

        for (var i = pardNode.Index; i < MainGroup.ChildNodes.Count; i++)
        {
            res.Append(MainGroup.ChildNodes[i].Text);
        }

        return res.ToString();
    }

    private RtfStyleSheet ParseStyleSheet(RtfTreeNode ssnode)
    {
        var rss = new RtfStyleSheet();

        foreach (RtfTreeNode node in ssnode.ChildNodes)
        {
            if (node.NodeKey == "cs")
            {
                rss.Type = RtfStyleSheetType.Character;
                rss.Index = node.Parameter;
            }
            else if (node.NodeKey == "s")
            {
                rss.Type = RtfStyleSheetType.Paragraph;
                rss.Index = node.Parameter;
            }
            else if (node.NodeKey == "ds")
            {
                rss.Type = RtfStyleSheetType.Section;
                rss.Index = node.Parameter;
            }
            else if (node.NodeKey == "ts")
            {
                rss.Type = RtfStyleSheetType.Table;
                rss.Index = node.Parameter;
            }
            else if (node.NodeKey == "additive")
            {
                rss.Additive = true;
            }
            else if (node.NodeKey == "sbasedon")
            {
                rss.BasedOn = node.Parameter;
            }
            else if (node.NodeKey == "snext")
            {
                rss.Next = node.Parameter;
            }
            else if (node.NodeKey == "sautoupd")
            {
                rss.AutoUpdate = true;
            }
            else if (node.NodeKey == "shidden")
            {
                rss.Hidden = true;
            }
            else if (node.NodeKey == "slink")
            {
                rss.Link = node.Parameter;
            }
            else if (node.NodeKey == "slocked")
            {
                rss.Locked = true;
            }
            else if (node.NodeKey == "spersonal")
            {
                rss.Personal = true;
            }
            else if (node.NodeKey == "scompose")
            {
                rss.Compose = true;
            }
            else if (node.NodeKey == "sreply")
            {
                rss.Reply = true;
            }
            else if (node.NodeKey == "styrsid")
            {
                rss.Styrsid = node.Parameter;
            }
            else if (node.NodeKey == "ssemihidden")
            {
                rss.SemiHidden = true;
            }
            else if (node.NodeType == RtfNodeType.Group &&
                     (node.ChildNodes[0].NodeKey == "*" && node.ChildNodes[1].NodeKey == "keycode"))
            {
                rss.KeyCode = new RtfNodeCollection();

                for (var i = 2; i < node.ChildNodes.Count; i++)
                {
                    rss.KeyCode.Add(node.ChildNodes[i].CloneNode());
                }
            }
            else if (node.NodeType == RtfNodeType.Text)
            {
                rss.Name = node.NodeKey.Substring(0,node.NodeKey.Length-1);
            }
            else
            {
                if(node.NodeKey != "*")
                    rss.Formatting.Add(node);
            }
        }

        return rss;
    }

    public RtfTreeNode RootNode
    {
        get
        {
            return rootNode;
        }
    }

    public RtfTreeNode MainGroup
    {
        get
        { 
            if (rootNode.HasChildNodes())
                return rootNode.ChildNodes[0];
            else
                return null;
        }
    }

    public string Rtf
    {
        get
        {
            return rootNode.Rtf;
        }
    }

    public bool MergeSpecialCharacters
    {
        get
        {
            return mergeSpecialCharacters;
        }
        set
        {
            mergeSpecialCharacters = value;
        }
    }

    public string Text
    {
        get
        {
            return ConvertToText();
        }
    }

}