using System.Text.RegularExpressions;
using HtmlAgilityPack;
using RtfToHtml;

namespace BiblicalBytes.Converters.RtfToHtml;

internal class Rtf
{
    private Table table;

    private readonly string rtfHeaderOpening;
    private string rtfHeaderContent;
    private readonly string rtfClosing;
    private HtmlNode prevTag;

    private List<Reference> rtfContentReferences = new List<Reference>();
    public Rtf()
    {
        this.rtfHeaderOpening = "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2";
        this.rtfHeaderContent = "";
        this.rtfClosing = "}";
    }
    public string ConvertHtmlToRtf(string html)
    {
        var htmlWithoutStrangerTags = this.SwapHtmlStrangerTags(html);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlWithoutStrangerTags);
        var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//html");
        var treeofTags = htmlBody.ChildNodes;
        Style.GetRtfReferenceColor("rgb(0,0,0)");
        Style.GetRtfReferenceColor("rgb(255,255,255)");

        foreach (var node in treeofTags)
        {
            ReadAllChildsInTag(node);
        }
        return this.BuildRtf();
    }

    private void ReadAllChildsInTag(HtmlNode fatherTag)
    {

        if (fatherTag.ChildNodes != null)
        {

            this.AddOpeningTagInRtfCode(fatherTag.Name);
            this.IfExistsAttributesAddAllReferencesInRtfCode(fatherTag.Attributes);

            if (fatherTag.Name.ToLower() == "table")
            {
                table = new Table();
                table.SetAmountOfColumns(this.GetAmountOfColumnThroughOfFirstChildOfTbodyTag(fatherTag.ChildNodes));
            }

            if (fatherTag.Name.ToLower() == "tr")
            {
                if (table != null)
                    this.AddReferenceTagInRtfCode(table.BuildCellsLengthOfEachColumn());
            }

            //if (fatherTag.Name.toLowerCase() == 'mark')
            //    this.setHighlightInRtf();
            foreach (var node in fatherTag.ChildNodes)
            {

                if (node.OriginalName != "#text")
                {

                    this.ReadAllChildsInTag(node);
                }
                else
                {
                    if (node.NextSibling != null)
                        if ((node.NextSibling.Name.ToLower() == "p"
                             || node.NextSibling.Name.ToLower() == "h1"
                             || node.NextSibling.Name.ToLower() == "h2"
                             || node.NextSibling.Name.ToLower() == "h3"
                             || node.NextSibling.Name.ToLower() == "h4"
                             || node.NextSibling.Name.ToLower() == "h5"
                             || node.NextSibling.Name.ToLower() == "h6")&& !String.IsNullOrWhiteSpace(node.InnerText))
                        {
                            var text = node.InnerText;
                            text = MyString.RemoveCharacterOfEscapeInAllString(text, "\n\t");

                            if (text != null && !MyString.hasOnlyWhiteSpace(text))
                                this.rtfContentReferences.Add(new Reference() { content = this.AddSpaceAroundString(text)+"{\\par}", tag = false });

                            //this.addContentOfTagInRtfCode(text.Trim() + "{\\par}");

                        }
                        else
                        {
                            this.AddContentOfTagInRtfCode(node.InnerText,"text");

                        }
                    else
                    {
                        this.AddContentOfTagInRtfCode(node.InnerText,"text");

                    }

                    //Console.WriteLine(node.InnerText);
                }
            }

        }
        //Console.WriteLine(fatherTag.OriginalName);
        if (fatherTag.OriginalName == "a" && !fatherTag.Attributes.Contains("href"))
        {
            this.AddReferenceTagInRtfCode("}");
        }
        else
        {
            this.AddClosingFatherTagInRtfCode(fatherTag.OriginalName);
        }
        prevTag = fatherTag;
    }

    private int GetAmountOfColumnThroughOfFirstChildOfTbodyTag(HtmlNodeCollection childs)
    {
        var count = 0;
        HtmlNode tbodyIndex;
        foreach (var element in childs)
        {
            if (element.Name == "tbody")
            {
                tbodyIndex = element;
                foreach (var node in element.ChildNodes)
                {

                    if (node.OriginalName != "#text")
                    {
                        foreach (var tr in node.ChildNodes)
                        {
                            if (tr.OriginalName != "#text")
                                count++;
                        }
                        break;
                    }
                }
                break;

            }

        }
        // Console.WriteLine(count);
        return count;

    }

    private void IfExistsAttributesAddAllReferencesInRtfCode(HtmlAttributeCollection attributes)
    {
        foreach (var attribute in attributes)
        {
            if (attribute.OriginalName == "align" || attribute.OriginalName == "text-align")
            {
                this.AddReferenceTagInRtfCode(Style.GetRtfAlignmentReference(attribute.Value));
            }
                
            if (attribute.OriginalName == "style")
            {
                this.AddReferenceTagInRtfCode(Style.GetRtfReferencesInStyleProperty(attribute.Value));
            }
            if (attribute.OriginalName == "href")
            {
                this.AddReferenceTagInRtfCode("{\\field{\\*\\fldinst HYPERLINK " + @"""" + attribute.Value + @"""" + "}{\\fldrslt{\\ul\\cf3");
            }
        }
        //if (attributes.s != null)
        //    this.addReferenceTagInRtfCode(Style.getRtfReferencesInStyleProperty(attributes.style));

    }


    private void AddClosingFatherTagInRtfCode(string closingFatherTag)
    {
        this.AddReferenceTagInRtfCode(AllowedHtmlTags.GetRtfReferenceTag($"/{closingFatherTag}"));
    }

    private void AddContentOfTagInRtfCode(string contentOfTag,string type = "notText")
    {
           
        contentOfTag = MyString.RemoveCharacterOfEscapeInAllString(contentOfTag, "\n\t");

        if (contentOfTag != null && !MyString.hasOnlyWhiteSpace(contentOfTag))
            this.rtfContentReferences.Add(new Reference() { content = this.AddSpaceAroundString(contentOfTag), tag = false });
            
    }

    private string AddSpaceAroundString(string contentOfTag)
    {
        Console.WriteLine(contentOfTag + "wrapped");
        if (this.rtfContentReferences.Last().content.Contains('\\'))
        {
            Console.WriteLine(this.rtfContentReferences.Last().content);
            return $" {contentOfTag}";
        }
        else
            return $"{contentOfTag}";
    }

    private void AddOpeningTagInRtfCode(string tag)
    {
        this.AddReferenceTagInRtfCode(AllowedHtmlTags.GetRtfReferenceTag(tag));
    }

    private void AddReferenceTagInRtfCode(string referenceTag)
    {
        if (referenceTag != null)
        {
            this.rtfContentReferences.Add(new Reference { content = referenceTag, tag = true });
        }
    }

    private string BuildRtf()
    {
        this.rtfHeaderContent += Style.GetRtfFontTable();
        this.rtfHeaderContent += Style.GetRtfColorTable();
        var content = (this.rtfHeaderOpening + this.rtfHeaderContent
                                             + this.GetRtfContentReferences()
                                             + this.rtfClosing);
        this.ClearCacheContent();
        return content;
    }

    private void ClearCacheContent()
    {
        this.rtfHeaderContent = "";
        this.rtfContentReferences = new List<Reference>();
    }

    private string GetRtfContentReferences()
    {
        var rtfReference = "";
        foreach (var value in this.rtfContentReferences)
        {

            rtfReference += value.content;
        }
        return rtfReference;
    }

    private string SwapHtmlStrangerTags(string html)
    {
        var evaluator = new MatchEvaluator(WordReplace);
        var replace = html.Replace("&nbsp;", @"{\*\htmltag &nbsp;}");
        Console.WriteLine(replace);

        replace = Regex.Replace(replace, @"<(\/?[a-z-]+)( *[^>]*)?>", evaluator, RegexOptions.IgnoreCase);

        return replace;
    }
    public static string WordReplace(Match match)
    {
        var evaluator1 = new MatchEvaluator(CheckTag);


        var tag = Regex.Replace(match.Value, @"<(\/?[a-z-]+[0-9]?)", evaluator1, RegexOptions.IgnoreCase);


        return tag;
    }
    public static string CheckTag(Match match)
    {
        var tag = match.Value;
        if (match.Value.Contains('/'))
        {
            tag = tag.Remove(0, 1);


            if (!AllowedHtmlTags.IsKnowedTag(tag))
            {

                return "</html";
            }
            else return match.Value;
        }
        else
        {
            tag = tag.Remove(0, 1);

            if (!AllowedHtmlTags.IsKnowedTag(tag))
            {

                return "<html";
            }
            else return match.Value;
        }

    }
}