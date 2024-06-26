using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfTreeNode
{
    private RtfNodeCollection children;

    #region Constructors
    public RtfTreeNode()
    {
        NodeType = RtfNodeType.None;
        NodeKey = "";
                
    }

    public RtfTreeNode(RtfNodeType nodeType)
    {
        NodeType = nodeType;
        NodeKey = "";

        if (nodeType == RtfNodeType.Group || nodeType == RtfNodeType.Root)
            children = new RtfNodeCollection();

        if (nodeType == RtfNodeType.Root)
            RootNode = this;
                
    }

    public RtfTreeNode(RtfNodeType type, string key, bool hasParameter, int parameter)
    {
        this.NodeType = type;
        this.NodeKey = key;
        HasParameter = hasParameter;
        Parameter = parameter;

        if (type == RtfNodeType.Group || type == RtfNodeType.Root)
            children = new RtfNodeCollection();

        if (type == RtfNodeType.Root)
            RootNode = this;

    }

    internal RtfTreeNode(RtfToken token)
    {
        NodeType = (RtfNodeType)token.Type;
        NodeKey = token.Key;
        HasParameter = token.HasParameter;
        Parameter = token.Parameter;

    }
    #endregion

    public RtfTreeNode RootNode { get; set; }

    public RtfTreeNode ParentNode { get; set; }

    public RtfTree Tree { get; set; }

    public RtfNodeType NodeType { get; set; }

    public string NodeKey { get; set; }

    public bool HasParameter { get; set; }

    public int Parameter { get; set; }

    public string Rtf
    {
        get
        {
            return GetRtf();
        }
    }

    public int Index
    {
        get
        {
            var res = -1;

            if (ParentNode != null)
                res = ParentNode.children.IndexOf(this);

            return res;
        }
    }

    public string Text
    {
        get
        {
            return GetText(false);
        }
    }

    public string RawText
    {
        get
        {
            return GetText(true);
        }
    }

    #region Navigation and Node Selection

    public RtfNodeCollection ChildNodes
    {
        get
        {
            return children;
        }
        set
        {
            children = value;

            foreach (RtfTreeNode node in children)
            {
                node.ParentNode = this;

                UpdateNodeRoot(node);
            }
        }
    }

    public RtfTreeNode this[string keyword]
    {
        get
        {
            return SelectSingleChildNode(keyword);
        }
    }

    public RtfTreeNode this[int childIndex]
    {
        get
        { 
            RtfTreeNode res = null;

            if (children != null && childIndex >= 0 && childIndex < children.Count)
                res = children[childIndex];

            return res;
        }
    }

    public RtfTreeNode FirstChild
    {
        get
        {
            RtfTreeNode res = null;

            if (children != null && children.Count > 0)
                res = children[0];

            return res;
        }
    }

    public RtfTreeNode LastChild
    {
        get
        {
            RtfTreeNode res = null;

            if (children != null && children.Count > 0)
                return children[children.Count - 1];

            return res;
        }
    }

    public RtfTreeNode NextSibling
    {
        get
        {
            RtfTreeNode res = null;

            if (ParentNode != null && ParentNode.children != null)
            {
                var currentIndex = ParentNode.children.IndexOf(this);

                if (ParentNode.children.Count > currentIndex + 1)
                    res = ParentNode.children[currentIndex + 1];
            }

            return res;
        }
    }

    public RtfTreeNode PreviousSibling
    {
        get
        {
            RtfTreeNode res = null;

            if (ParentNode != null && ParentNode.children != null)
            {
                var currentIndex = ParentNode.children.IndexOf(this);

                if (currentIndex > 0)
                    res = ParentNode.children[currentIndex - 1];
            }

            return res;
        }
    }

    public RtfTreeNode NextNode
    {
        get
        {
            RtfTreeNode res = null;

            if (NodeType == RtfNodeType.Root)
            {
                res = FirstChild;
            }
            else if (ParentNode != null && ParentNode.children != null)
            {
                if (NodeType == RtfNodeType.Group && children.Count > 0)
                {
                    res = FirstChild;
                }
                else
                {
                    if (Index < (ParentNode.children.Count - 1))
                    {
                        res = NextSibling;
                    }
                    else
                    {
                        res = ParentNode.NextSibling;
                    }
                }
            }

            return res;
        }
    }

    public RtfTreeNode PreviousNode
    {
        get
        {
            RtfTreeNode res = null;

            if (NodeType == RtfNodeType.Root)
            {
                res = null;
            }
            else if (ParentNode != null && ParentNode.children != null)
            {
                if (Index > 0)
                {
                    if (PreviousSibling.NodeType == RtfNodeType.Group)
                    {
                        res = PreviousSibling.LastChild;
                    }
                    else
                    {
                        res = PreviousSibling;
                    }
                }
                else
                {
                    res = ParentNode;
                }
            }

            return res;
        }
    }

    public void AppendChild(RtfTreeNode newNode)
    {
        if (newNode != null)
        {
            if (children == null)
                children = new RtfNodeCollection();

            newNode.ParentNode = this;

            UpdateNodeRoot(newNode);

            children.Add(newNode);
        }
    }

    public void InsertChild(int index, RtfTreeNode newNode)
    {
        if (newNode != null)
        {
            if (children == null)
                children = new RtfNodeCollection();

            if (index >= 0 && index <= children.Count)
            {
                newNode.ParentNode = this;

                UpdateNodeRoot(newNode);

                children.Insert(index, newNode);
            }
        }
    }

    public void RemoveChild(int index)
    {
        if (children != null)
        {
            if (index >= 0 && index < children.Count)
            {
                children.RemoveAt(index);
            }
        }
    }

    public void RemoveChild(RtfTreeNode node)
    {
        if (children != null)
        {
            var index = children.IndexOf(node);

            if (index != -1)
            {
                children.RemoveAt(index);
            }
        }
    }

    public RtfTreeNode CloneNode()
    {
        var clon = new RtfTreeNode();

        clon.NodeKey = NodeKey;
        clon.HasParameter = HasParameter;
        clon.Parameter = Parameter;
        clon.ParentNode = null;
        clon.RootNode = null;
        clon.Tree = null;
        clon.NodeType = NodeType;

        clon.children = null;

        if (children != null)
        {
            clon.children = new RtfNodeCollection();

            foreach (RtfTreeNode child in children)
            {
                var childclon = child.CloneNode();
                childclon.ParentNode = clon;

                clon.children.Add(childclon);
            }
        }

        return clon;
    }

    public bool HasChildNodes()
    {
        var res = false;

        if (children != null && children.Count > 0)
            res = true;

        return res;
    }

    public RtfTreeNode SelectSingleChildNode(string keyword)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeKey == keyword)
                {
                    node = children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleChildNode(RtfNodeType nodeType)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeType == nodeType)
                {
                    node = children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleChildNode(string keyword, int param)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeKey == keyword && children[i].Parameter == param)
                {
                    node = children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleChildGroup(string keyword)
    {
        return SelectSingleChildGroup(keyword, false);
    }

    public RtfTreeNode SelectSingleChildGroup(string keyword, bool ignoreSpecial)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeType == RtfNodeType.Group && children[i].HasChildNodes() &&
                    (
                        (children[i].FirstChild.NodeKey == keyword) ||
                        (ignoreSpecial && children[i].ChildNodes[0].NodeKey == "*" && children[i].ChildNodes[1].NodeKey == keyword))
                   )
                {
                    node = children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleNode(RtfNodeType nodeType)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeType == nodeType)
                {
                    node = children[i];
                    found = true;
                }
                else
                {
                    node = children[i].SelectSingleNode(nodeType);

                    if (node != null)
                    {
                        found = true;
                    }
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleNode(string keyword)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeKey == keyword)
                {
                    node = children[i];
                    found = true;
                }
                else
                {
                    node = children[i].SelectSingleNode(keyword);

                    if (node != null)
                    {
                        found = true;
                    }
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleGroup(string keyword)
    {
        return SelectSingleGroup(keyword, false);
    }

    public RtfTreeNode SelectSingleGroup(string keyword, bool ignoreSpecial)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeType == RtfNodeType.Group && children[i].HasChildNodes() &&
                    (
                        (children[i].FirstChild.NodeKey == keyword) ||
                        (ignoreSpecial && children[i].ChildNodes[0].NodeKey == "*" && children[i].ChildNodes[1].NodeKey == keyword))
                   )
                {
                    node = children[i];
                    found = true;
                }
                else
                {
                    node = children[i].SelectSingleGroup(keyword, ignoreSpecial);

                    if (node != null)
                    {
                        found = true;
                    }
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSingleNode(string keyword, int param)
    {
        var i = 0;
        var found = false;
        RtfTreeNode node = null;

        if (children != null)
        {
            while (i < children.Count && !found)
            {
                if (children[i].NodeKey == keyword && children[i].Parameter == param)
                {
                    node = children[i];
                    found = true;
                }
                else
                {
                    node = children[i].SelectSingleNode(keyword, param);

                    if (node != null)
                    {
                        found = true;
                    }
                }

                i++;
            }
        }

        return node;
    }

    public RtfNodeCollection SelectNodes(string keyword)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeKey == keyword)
                {
                    nodes.Add(node);
                }

                nodes.AddRange(node.SelectNodes(keyword));
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectGroups(string keyword)
    {
        return SelectGroups(keyword, false);
    }

    public RtfNodeCollection SelectGroups(string keyword, bool ignoreSpecial)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == RtfNodeType.Group && node.HasChildNodes() &&
                    (
                        (node.FirstChild.NodeKey == keyword) ||
                        (ignoreSpecial && node.ChildNodes[0].NodeKey == "*" && node.ChildNodes[1].NodeKey == keyword))
                   )
                {
                    nodes.Add(node);
                }

                nodes.AddRange(node.SelectGroups(keyword, ignoreSpecial));
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectNodes(RtfNodeType nodeType)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == nodeType)
                {
                    nodes.Add(node);
                }

                nodes.AddRange(node.SelectNodes(nodeType));
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectNodes(string keyword, int param)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeKey == keyword && node.Parameter == param)
                {
                    nodes.Add(node);
                }

                nodes.AddRange(node.SelectNodes(keyword, param));
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectChildNodes(string keyword)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeKey == keyword)
                {
                    nodes.Add(node);
                }
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectChildGroups(string keyword)
    {
        return SelectChildGroups(keyword, false);
    }

    public RtfNodeCollection SelectChildGroups(string keyword, bool ignoreSpecial)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == RtfNodeType.Group && node.HasChildNodes() &&
                    (
                        (node.FirstChild.NodeKey == keyword) ||
                        (ignoreSpecial && node.ChildNodes[0].NodeKey == "*" && node.ChildNodes[1].NodeKey == keyword))
                   )
                {
                    nodes.Add(node);
                }
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectChildNodes(RtfNodeType nodeType)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == nodeType)
                {
                    nodes.Add(node);
                }
            }
        }

        return nodes;
    }

    public RtfNodeCollection SelectChildNodes(string keyword, int param)
    {
        var nodes = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeKey == keyword && node.Parameter == param)
                {
                    nodes.Add(node);
                }
            }
        }

        return nodes;
    }

    public RtfTreeNode SelectSibling(string keyword)
    {
        RtfTreeNode node = null;
        var par = ParentNode;

        if (par != null)
        {
            var curInd = par.ChildNodes.IndexOf(this);

            var i = curInd + 1;
            var found = false;

            while (i < par.children.Count && !found)
            {
                if (par.children[i].NodeKey == keyword)
                {
                    node = par.children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSibling(RtfNodeType nodeType)
    {
        RtfTreeNode node = null;
        var par = ParentNode;

        if (par != null)
        {
            var curInd = par.ChildNodes.IndexOf(this);

            var i = curInd + 1;
            var found = false;

            while (i < par.children.Count && !found)
            {
                if (par.children[i].NodeType == nodeType)
                {
                    node = par.children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }

    public RtfTreeNode SelectSibling(string keyword, int param)
    {
        RtfTreeNode node = null;
        var par = ParentNode;

        if (par != null)
        {
            var curInd = par.ChildNodes.IndexOf(this);

            var i = curInd + 1;
            var found = false;

            while (i < par.children.Count && !found)
            {
                if (par.children[i].NodeKey == keyword && par.children[i].Parameter == param)
                {
                    node = par.children[i];
                    found = true;
                }

                i++;
            }
        }

        return node;
    }
#endregion

    public RtfNodeCollection FindText(string text)
    {
        var list = new RtfNodeCollection();

        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == RtfNodeType.Text && node.NodeKey.IndexOf(text) != -1)
                    list.Add(node);
                else if (node.NodeType == RtfNodeType.Group)
                    list.AddRange(node.FindText(text));
            }
        }

        return list;
    }

    public void ReplaceText(string oldValue, string newValue)
    {
        if (children != null)
        {
            foreach (RtfTreeNode node in children)
            {
                if (node.NodeType == RtfNodeType.Text)
                    node.NodeKey = node.NodeKey.Replace(oldValue, newValue);
                else if (node.NodeType == RtfNodeType.Group)
                    node.ReplaceText(oldValue, newValue);
            }
        }
    }

    public override string ToString()
    {
        return "[" + NodeType + ", " + NodeKey + ", " + HasParameter + ", " + Parameter + "]";
    }

    private string DecodeControlChar(int code, Encoding enc)
    {
        return enc.GetString(new byte[] { (byte)code });
    }

    private string GetRtf()
    {
        var res = "";

        var enc = Tree.GetEncoding();

        res = GetRtfInm(this, null, enc);

        return res;
    }

    private string GetRtfInm(RtfTreeNode curNode, RtfTreeNode prevNode, Encoding enc)
    {
        var res = new StringBuilder("");

        if (curNode.NodeType == RtfNodeType.Root)
            res.Append("");
        else if (curNode.NodeType == RtfNodeType.Group)
            res.Append("{");
        else
        {
            if (curNode.NodeType == RtfNodeType.Control ||
                curNode.NodeType == RtfNodeType.Keyword)
            {
                res.Append("\\");
            }
            else
            {
                if (prevNode != null &&
                    prevNode.NodeType == RtfNodeType.Keyword)
                {
                    var code = Char.ConvertToUtf32(curNode.NodeKey, 0);

                    if (code >= 32 && code < 128)
                        res.Append(" ");
                }
            }

            AppendEncoded(res, curNode.NodeKey, enc);

            if (curNode.HasParameter)
            {
                if (curNode.NodeType == RtfNodeType.Keyword)
                {
                    res.Append(Convert.ToString(curNode.Parameter));
                }
                else if (curNode.NodeType == RtfNodeType.Control)
                {
                    if (curNode.NodeKey == "\'")
                    {
                        res.Append(GetHexa(curNode.Parameter));
                    }
                }
            }
        }

        var children = curNode.ChildNodes;

        if (children != null)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var node = children[i];

                if (i > 0)
                    res.Append(GetRtfInm(node, children[i - 1], enc));
                else
                    res.Append(GetRtfInm(node, null, enc));
            }
        }

        if (curNode.NodeType == RtfNodeType.Group)
        {
            res.Append("}");
        }

        return res.ToString();
    }

    private void AppendEncoded(StringBuilder res, string s, Encoding enc)
    {
        for (var i = 0; i < s.Length; i++)
        {
            var code = Char.ConvertToUtf32(s, i);

            if (code >= 128 || code < 32)
            {
                res.Append(@"\'");
                var bytes = enc.GetBytes(new char[] { s[i] });
                res.Append(GetHexa(bytes[0]));
            }
            else
            {
                if ((s[i] == '{') || (s[i] == '}') || (s[i] == '\\'))
                {
                    res.Append(@"\");
                }

                res.Append(s[i]);
            }
        }
    }

    private string GetHexa(int code)
    {
        var hexa = Convert.ToString(code, 16);

        if (hexa.Length == 1)
        {
            hexa = "0" + hexa;
        }

        return hexa;
    }

    private void UpdateNodeRoot(RtfTreeNode node)
    {
        node.RootNode = RootNode;

        node.Tree = Tree;

        if (node.children != null)
        {
            foreach (RtfTreeNode nod in node.children)
            {
                UpdateNodeRoot(nod);
            }
        }
    }

    private string GetText(bool raw)
    {
        return GetText(raw, 1);
    }

    private string GetText(bool raw, int ignoreNchars)
    {
        var res = new StringBuilder("");

        if (NodeType == RtfNodeType.Group)
        {
            var indkw = FirstChild.NodeKey.Equals("*") ? 1 : 0;

            if (raw ||
                (!ChildNodes[indkw].NodeKey.Equals("fonttbl") &&
                 !ChildNodes[indkw].NodeKey.Equals("colortbl") &&
                 !ChildNodes[indkw].NodeKey.Equals("stylesheet") &&
                 !ChildNodes[indkw].NodeKey.Equals("generator") &&
                 !ChildNodes[indkw].NodeKey.Equals("info") &&
                 !ChildNodes[indkw].NodeKey.Equals("pict") &&
                 !ChildNodes[indkw].NodeKey.Equals("object") &&
                 !ChildNodes[indkw].NodeKey.Equals("fldinst")))
            {
                if (ChildNodes != null)
                {
                    var uc = ignoreNchars;
                    foreach (RtfTreeNode node in ChildNodes)
                    {
                        res.Append(node.GetText(raw, uc));

                        if (node.NodeType == RtfNodeType.Keyword && node.NodeKey.Equals("uc"))
                            uc = node.Parameter;
                    }
                }
            }
        }
        else if (NodeType == RtfNodeType.Control)
        {
            if (NodeKey == "'")
                res.Append(DecodeControlChar(Parameter, Tree.GetEncoding()));
            else if (NodeKey == "~")
                res.Append(" ");
        }
        else if (NodeType == RtfNodeType.Text)
        {
            var newtext = NodeKey;

            if (PreviousNode.NodeType == RtfNodeType.Keyword &&
                PreviousNode.NodeKey.Equals("u"))
            {
                newtext = newtext.Substring(ignoreNchars);
            }

            res.Append(newtext);
        }
        else if (NodeType == RtfNodeType.Keyword)
        {
            if (NodeKey.Equals("par"))
                res.AppendLine("");
            else if (NodeKey.Equals("tab"))
                res.Append("\t");
            else if (NodeKey.Equals("line"))
                res.AppendLine("");
            else if (NodeKey.Equals("lquote"))
                res.Append("‘");
            else if (NodeKey.Equals("rquote"))
                res.Append("’");
            else if (NodeKey.Equals("ldblquote"))
                res.Append("“");
            else if (NodeKey.Equals("rdblquote"))
                res.Append("”");
            else if (NodeKey.Equals("emdash"))
                res.Append("—");
            else if (NodeKey.Equals("u"))
            {
                res.Append(Char.ConvertFromUtf32(Parameter));
            }
        }

        return res.ToString();
    }
}