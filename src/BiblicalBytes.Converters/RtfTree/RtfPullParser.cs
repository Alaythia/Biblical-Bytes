namespace BiblicalBytes.Converters.RtfTree;

public class RtfPullParser
{
    public const int StartDocument = 0;
    public const int EndDocument = 1;
    public const int Keyword = 2;
    public const int Control = 3;
    public const int StartGroup = 4;
    public const int EndGroup = 5;
    public const int Text = 6;

    private TextReader rtf;		   
    private RtfLex lex;		       
    private RtfToken tok;		 
    private int currentEvent;    

    public RtfPullParser()
    {
        currentEvent = StartDocument;
    }

    public int LoadRtfFile(string path)
    {
        var res = 0;

        rtf = new StreamReader(path);

        lex = new RtfLex(rtf);

        return res;
    }

    public int LoadRtfText(string text)
    {
        var res = 0;

        rtf = new StringReader(text);

        lex = new RtfLex(rtf);

        return res;
    }

    public int GetEventType()
    {
        return currentEvent;
    }

    public int Next()
    {
        tok = lex.NextToken();

        switch (tok.Type)
        {
            case RtfTokenType.GroupStart:
                currentEvent = StartGroup;
                break;
            case RtfTokenType.GroupEnd:
                currentEvent = EndGroup;
                break;
            case RtfTokenType.Keyword:
                currentEvent = Keyword;
                break;
            case RtfTokenType.Control:
                currentEvent = Control;
                break;
            case RtfTokenType.Text:
                currentEvent = Text;
                break;
            case RtfTokenType.Eof:
                currentEvent = EndDocument;
                break;
        }

        return currentEvent;
    }

    public string GetName()
    {
        return tok.Key;
    }

    public int GetParam()
    {
        return tok.Parameter;
    }

    public bool HasParam()
    {
        return tok.HasParameter;
    }

    public string GetText()
    {
        return tok.Key;
    }

}