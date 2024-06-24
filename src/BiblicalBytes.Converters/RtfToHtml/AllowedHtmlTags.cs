namespace BiblicalBytes.Converters.RtfToHtml;

public static class AllowedHtmlTags
{
    public static Tag[] HtmlTags =
    [
        new Tag()
        {
            Opening = "b",
            OpeningRtf = "{\\b",
            Closing = "/b",
            ClosingRtf =  "}"
        },
        new Tag()
        {
            Opening = "p",
            OpeningRtf= "{",
            Closing= "/p",
            ClosingRtf= "\\par}"
        },
        new Tag()
        {
            Opening= "html",
            OpeningRtf= "",
            Closing= "/html",
            ClosingRtf= ""
        },
        new Tag()
        {
            Opening= "head",
            OpeningRtf= "",
            Closing= "/head",
            ClosingRtf= ""
        },
        new Tag()
        {
            Opening= "body",
            OpeningRtf= "",
            Closing= "/body",
            ClosingRtf= ""
        },
        new Tag()
        {
            Opening="br",
            OpeningRtf="\\par",
            Closing="br/",
            ClosingRtf="\\par"
        },
        new Tag()
        {
            Opening="center",
            OpeningRtf="\\line{\\line\\qc",
            Closing="/center",
            ClosingRtf="\\par}"
        },
        new Tag()
        {
            Opening="div",
            OpeningRtf="{",
            Closing="/div",
            ClosingRtf= "\\par}"

        },
       
        new Tag()
        {
            Opening="a",
            OpeningRtf="",
            Closing="/a",
            ClosingRtf= "}}}"

        },
        new Tag()
        {
            Opening="em",
            OpeningRtf="{\\i",
            Closing="/em",
            ClosingRtf="}"
        },
        new Tag()
        {
            Opening="font",
            OpeningRtf="{",
            Closing="/font",
            ClosingRtf="\\line}"
        },
        new Tag()
        {
            Opening="h1",
            OpeningRtf="{\\fs43\\f2\\b {\\ltrch",
            Closing="/h1",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="h2",
            OpeningRtf="{\\fs39\\f2\\b {\\ltrch",
            Closing="/h2",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="h3",
            OpeningRtf="{\\fs36\\f2\\b {\\ltrch",
            Closing="/h3",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="h4",
            OpeningRtf="{\\fs32\\f2\\b {\\ltrch",
            Closing="/h4",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="h5",
            OpeningRtf="{\\fs24\\f2\\b {\\ltrch",
            Closing="/h5",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="h6",
            OpeningRtf="{\\fs19\\f2 {\\ltrch",
            Closing="/h6",
            ClosingRtf="}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}"
        },
        new Tag()
        {
            Opening="i",
            OpeningRtf="{\\i",
            Closing="/i",
            ClosingRtf="}"
        },
        new Tag()
        {
            Opening="li",
            OpeningRtf="{{\\pntext\\tab}",
            Closing="/li",
            ClosingRtf="}\\par"
        },
        new Tag()
        {
            Opening="mark",
            OpeningRtf="{",
            Closing="/mark",
            ClosingRtf="}"
        },
        new Tag()

        {
            Opening= "ol",
            OpeningRtf= "{{\\*\\pn\\pnlvlbody\\pnf0\\pnindent0\\pnstart1\\pndec{\\pntxta.}}\\fi-360\\li720\\sa200\\sl276\\slmult1",
            Closing= "/ol",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "ul",
            OpeningRtf= "{{\\*\\pn\\pnlvlblt\\pnf1\\pnindent0{\\pntxtb\\\'B7}}\\fi-360\\li720\\sa200\\sl276\\slmult1\\lang22\\f0\\fs22",
            Closing= "/ul",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "s",
            OpeningRtf= "{\\strike",
            Closing= "/s",
            ClosingRtf= "}"
        },
        new Tag()
        {
            Opening= "span",
            OpeningRtf= "{",
            Closing= "/span",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "sub",
            OpeningRtf= "{\\sub",
            Closing= "/sub",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "sup",
            OpeningRtf= "{\\super",
            Closing= "/sup",
            ClosingRtf= "\\line}"
        },
        new Tag()

        {
            Opening= "strong",
            OpeningRtf= "{\\b",
            Closing= "/strong",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "table",
            OpeningRtf= "\\par{",
            Closing= "/table",
            ClosingRtf= "}"
        },
        new Tag()

        {
            Opening= "tbody",
            OpeningRtf= "",
            Closing= "/tbody",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "thead",
            OpeningRtf= "",
            Closing= "/thead",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "td",
            OpeningRtf= "{\\line\\intbl\\qc",
            Closing= "/td",
            ClosingRtf= "\\cell}"
        },
        new Tag()

        {
            Opening= "th",
            OpeningRtf= "{\\line\\intbl\\qc",
            Closing= "/th",
            ClosingRtf= "\\cell}"
        },
        new Tag()

        {
            Opening= "tr",
            OpeningRtf= "{\\trowd\\trgaph10",
            Closing= "/tr",
            ClosingRtf= "\\row}"
        },
        new Tag()

        {
            Opening= "u",
            OpeningRtf= "{\\ul",
            Closing= "/u",
            ClosingRtf= "}"
        },
             
        new Tag()

        {
            Opening= "html",
            OpeningRtf= "",
            Closing= "/html",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "head",
            OpeningRtf= "",
            Closing= "/head",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "body",
            OpeningRtf= "",
            Closing= "/body",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "style",
            OpeningRtf= "",
            Closing= "/style",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "meta",
            OpeningRtf= "",
            Closing= "meta",
            ClosingRtf= ""
        },
        new Tag()

        {
            Opening= "title",
            OpeningRtf= "",
            Closing= "/title",
            ClosingRtf= ""
        }
    ];
    public static bool IsKnowedTag(string tag)
    {
        var isKnowedTag = GetAllowedTag(tag);
        return isKnowedTag != null;
    }
    public static Tag GetAllowedTag(string tag)
    {
        tag = tag.ToLower();
        foreach (var knowedTag in HtmlTags)
        {
            if (knowedTag.Opening == tag || knowedTag.Closing == tag)
                return knowedTag;
        }
        return null;
    }
    public static string GetRtfReferenceTag(string tagName)
    {
        Tag allowedTag;

        tagName = tagName.ToLower();
        allowedTag = GetAllowedTag(tagName);

        if (allowedTag != null)
        {
            return tagName == allowedTag.Opening ? allowedTag.OpeningRtf : allowedTag.ClosingRtf;
        }
        return null;
    }
}