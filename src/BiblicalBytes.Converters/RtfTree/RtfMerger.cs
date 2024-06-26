using System.Drawing;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfMerger
{
    private RtfTree baseRtfDoc;
    private bool removeLastPar;

    private readonly Dictionary<string, RtfTree> placeHolder;

    public RtfMerger(string templatePath)
    {
        baseRtfDoc = new RtfTree();
        baseRtfDoc.LoadRtfFile(templatePath);

        placeHolder = new Dictionary<string, RtfTree>();
    }

    public RtfMerger(RtfTree templateTree)
    {
        baseRtfDoc = templateTree;

        placeHolder = new Dictionary<string, RtfTree>();
    }

    public RtfMerger()
    {
        placeHolder = new Dictionary<string, RtfTree>();
    }

    public void AddPlaceHolder(string ph, string path)
    {
        var tree = new RtfTree();

        var res = tree.LoadRtfFile(path);

        if (res == 0)
        {
            placeHolder.Add(ph, tree);
        }
    }

    public void AddPlaceHolder(string ph, RtfTree docTree)
    {
        placeHolder.Add(ph, docTree);
    }

    public void RemovePlaceHolder(string ph)
    {
        placeHolder.Remove(ph);
    }

    public RtfTree Merge(bool removeLastPar)
    {
        this.removeLastPar = removeLastPar;

        var parentNode = baseRtfDoc.MainGroup;

        if (parentNode != null)
        {
            AnalizeTextContent(parentNode);
        }

        return baseRtfDoc;
    }

    public RtfTree Merge()
    {
        return Merge(true);
    }

    public Dictionary<string, RtfTree> Placeholders
    {
        get
        {
            return placeHolder;
        }
    }

    public RtfTree Template
    {
        get
        {
            return baseRtfDoc;
        }
        set
        {
            baseRtfDoc = value;
        }
    }

    private void AnalizeTextContent(RtfTreeNode parentNode)
    {
        RtfTree docToInsert = null;
        int indPh;

        if (parentNode != null && parentNode.HasChildNodes())
        {
            for (var iNdIndex = 0; iNdIndex < parentNode.ChildNodes.Count; iNdIndex++)
            {
                var currNode = parentNode.ChildNodes[iNdIndex];

                if (currNode.NodeType == RtfNodeType.Text)
                {
                    docToInsert = null;

                    foreach (var ph in placeHolder.Keys)
                    {
                        indPh = currNode.NodeKey.IndexOf(ph);

                        if (indPh != -1)
                        {
                            docToInsert = placeHolder[ph].CloneTree();

                            MergeCore(parentNode, iNdIndex, docToInsert, ph, indPh);

                            iNdIndex--;
                            break;
                        }
                    }
                }
                else
                {
                    if (currNode.HasChildNodes())
                    {
                        AnalizeTextContent(currNode);
                    }
                }
            }
        }
    }

    private void MergeCore(RtfTreeNode parentNode, int iNdIndex, RtfTree docToInsert, string strCompletePlaceholder, int intPlaceHolderNodePos)
    {
        if (docToInsert.RootNode.HasChildNodes())
        {
            var currentIndex = iNdIndex + 1;

            MainAdjustColor(docToInsert);

            MainAdjustFont(docToInsert);

            CleanToInsertDoc(docToInsert);

            if (docToInsert.RootNode.FirstChild.HasChildNodes())
            {
                ExecMergeDoc(parentNode, docToInsert, currentIndex);
            }

            if (parentNode.ChildNodes[iNdIndex].NodeKey.Length != (intPlaceHolderNodePos + strCompletePlaceholder.Length))
            {
                var remText = 
                    parentNode.ChildNodes[iNdIndex].NodeKey.Substring(
                        parentNode.ChildNodes[iNdIndex].NodeKey.IndexOf(strCompletePlaceholder) + strCompletePlaceholder.Length);

                parentNode.InsertChild(currentIndex + 1, new RtfTreeNode(RtfNodeType.Text, remText, false, 0));
            }

            if (intPlaceHolderNodePos == 0)
            {
                parentNode.RemoveChild(iNdIndex);
            }
            else  
            {
                parentNode.ChildNodes[iNdIndex].NodeKey = 
                    parentNode.ChildNodes[iNdIndex].NodeKey.Substring(0, intPlaceHolderNodePos);
            }
        }
    }

    private int GetFontId(ref RtfFontTable fontDestTbl, string sFontName)
    {
        var iExistingFontId = -1;

        if ((iExistingFontId = fontDestTbl.IndexOf(sFontName)) == -1)
        {
            fontDestTbl.AddFont(sFontName);
            iExistingFontId = fontDestTbl.IndexOf(sFontName);

            var nodeListToInsert = baseRtfDoc.RootNode.SelectNodes("fonttbl");

            var ftFont = new RtfTreeNode(RtfNodeType.Group);
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "f", true, iExistingFontId));
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "fnil", false, 0));
            ftFont.AppendChild(new RtfTreeNode(RtfNodeType.Text, sFontName + ";", false, 0));
                    
            nodeListToInsert[0].ParentNode.AppendChild(ftFont);
        }

        return iExistingFontId;
    }

    private int GetColorId(RtfColorTable colorDestTbl, Color iColorName)
    {
        int iExistingColorId;

        if ((iExistingColorId = colorDestTbl.IndexOf(iColorName)) == -1)
        {
            iExistingColorId = colorDestTbl.Count;
            colorDestTbl.AddColor(iColorName);

            var nodeListToInsert = baseRtfDoc.RootNode.SelectNodes("colortbl");

            nodeListToInsert[0].ParentNode.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "red", true, iColorName.R));
            nodeListToInsert[0].ParentNode.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "green", true, iColorName.G));
            nodeListToInsert[0].ParentNode.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "blue", true, iColorName.B));
            nodeListToInsert[0].ParentNode.AppendChild(new RtfTreeNode(RtfNodeType.Text, ";", false, 0));
        }

        return iExistingColorId;
    }

    private void MainAdjustFont(RtfTree docToInsert)
    {
        var fontDestTbl = baseRtfDoc.GetFontTable();
        var fontToCopyTbl = docToInsert.GetFontTable();

        AdjustFontRecursive(docToInsert.RootNode, fontDestTbl, fontToCopyTbl);
    }

    private void AdjustFontRecursive(RtfTreeNode parentNode, RtfFontTable fontDestTbl, RtfFontTable fontToCopyTbl)
    {
        if (parentNode != null && parentNode.HasChildNodes())
        {
            for (var iNdIndex = 0; iNdIndex < parentNode.ChildNodes.Count; iNdIndex++)
            {
                if (parentNode.ChildNodes[iNdIndex].NodeType == RtfNodeType.Keyword &&
                    (parentNode.ChildNodes[iNdIndex].NodeKey == "f" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "stshfdbch" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "stshfloch" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "stshfhich" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "stshfbi" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "deff" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "af") &&
                    parentNode.ChildNodes[iNdIndex].HasParameter)
                {
                    parentNode.ChildNodes[iNdIndex].Parameter = GetFontId(ref fontDestTbl, fontToCopyTbl[parentNode.ChildNodes[iNdIndex].Parameter]);
                }

                AdjustFontRecursive(parentNode.ChildNodes[iNdIndex], fontDestTbl, fontToCopyTbl);
            }
        }
    }

    private void MainAdjustColor(RtfTree docToInsert)
    {
        var colorDestTbl = baseRtfDoc.GetColorTable();
        var colorToCopyTbl = docToInsert.GetColorTable();

        AdjustColorRecursive(docToInsert.RootNode, colorDestTbl, colorToCopyTbl);
    }

    private void AdjustColorRecursive(RtfTreeNode parentNode, RtfColorTable colorDestTbl, RtfColorTable colorToCopyTbl)
    {
        if (parentNode != null && parentNode.HasChildNodes())
        {
            for (var iNdIndex = 0; iNdIndex < parentNode.ChildNodes.Count; iNdIndex++)
            {
                if (parentNode.ChildNodes[iNdIndex].NodeType == RtfNodeType.Keyword &&
                    (parentNode.ChildNodes[iNdIndex].NodeKey == "cf" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "cb" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "pncf" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "brdrcf" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "cfpat" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "cbpat" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "clcfpatraw" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "clcbpatraw" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "ulc" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "chcfpat" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "chcbpat" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "highlight" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "clcbpat" ||
                     parentNode.ChildNodes[iNdIndex].NodeKey == "clcfpat") &&
                    parentNode.ChildNodes[iNdIndex].HasParameter)
                {
                    parentNode.ChildNodes[iNdIndex].Parameter = GetColorId(colorDestTbl, colorToCopyTbl[parentNode.ChildNodes[iNdIndex].Parameter]);
                }

                AdjustColorRecursive(parentNode.ChildNodes[iNdIndex], colorDestTbl, colorToCopyTbl);
            }
        }
    }

    private void ExecMergeDoc(RtfTreeNode parentNode, RtfTree treeToCopyParent, int intCurrIndex)
    {
        var nodePard = treeToCopyParent.RootNode.FirstChild.SelectSingleChildNode("pard");

        var indPard = treeToCopyParent.RootNode.FirstChild.ChildNodes.IndexOf(nodePard);

        var newGroup = new RtfTreeNode(RtfNodeType.Group);

        newGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "pard", false, 0));
        newGroup.AppendChild(new RtfTreeNode(RtfNodeType.Keyword, "plain", false, 0));

        for (var i = indPard + 1; i < treeToCopyParent.RootNode.FirstChild.ChildNodes.Count; i++)
        {
            var newNode = 
                treeToCopyParent.RootNode.FirstChild.ChildNodes[i].CloneNode();

            newGroup.AppendChild(newNode);
        }

        parentNode.InsertChild(intCurrIndex, newGroup);
    }

    private void CleanToInsertDoc(RtfTree docToInsert)
    {
        var lastNode = docToInsert.RootNode.FirstChild.LastChild;

        if (removeLastPar)
        {
            if (lastNode.NodeType == RtfNodeType.Keyword && lastNode.NodeKey == "par")
            {
                docToInsert.RootNode.FirstChild.RemoveChild(lastNode);
            }
        }
    }

}