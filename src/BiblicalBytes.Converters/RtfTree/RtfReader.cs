namespace BiblicalBytes.Converters.RtfTree;

public class RtfReader
{
    private TextReader rtf;		   
    private RtfLex lex;		   
    private RtfToken tok;		 
    private readonly SarParser reader;		 

    public RtfReader(SarParser reader)
    {
        this.reader = reader;
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

    public int Parse()
    {
        var res = 0;

        reader.StartRtfDocument();

        tok = lex.NextToken();

        while (tok.Type != RtfTokenType.Eof)
        {
            switch (tok.Type)
            {
                case RtfTokenType.GroupStart:
                    reader.StartRtfGroup();
                    break;
                case RtfTokenType.GroupEnd:
                    reader.EndRtfGroup();
                    break;
                case RtfTokenType.Keyword:
                    reader.RtfKeyword(tok.Key, tok.HasParameter, tok.Parameter);
                    break;
                case RtfTokenType.Control:
                    reader.RtfControl(tok.Key, tok.HasParameter, tok.Parameter);
                    break;
                case RtfTokenType.Text:
                    reader.RtfText(tok.Key);
                    break;
                default:
                    res = -1;
                    break;
            }

            tok = lex.NextToken();
        }

        reader.EndRtfDocument();

        rtf.Close();

        return res;
    }

}