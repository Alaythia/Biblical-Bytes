using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class RtfLex
{
    private readonly TextReader rtf;

    private readonly StringBuilder keysb;

    private readonly StringBuilder parsb;

    private int c;

    private const int Eof = -1;

    public RtfLex(TextReader rtfReader)
    {
        rtf = rtfReader;

        keysb = new StringBuilder();
        parsb = new StringBuilder();

        c = rtf.Read();
    }

    public RtfToken NextToken()
    {
        var token = new RtfToken();

        while (c == '\r' || c == '\n' || c == '\t' || c == '\0')
            c = rtf.Read();

        if (c != Eof)
        {
            switch (c)
            {
                case '{':
                    token.Type = RtfTokenType.GroupStart;
                    c = rtf.Read();
                    break;
                case '}':
                    token.Type = RtfTokenType.GroupEnd;
                    c = rtf.Read();
                    break;
                case '\\':
                    ParseKeyword(token);
                    break;
                default:
                    token.Type = RtfTokenType.Text;
                    ParseText(token);
                    break;
            }
        }
        else
        {
            token.Type = RtfTokenType.Eof;
        }

        return token;
    }


    private void ParseKeyword(RtfToken token)
    {
        keysb.Length = 0;
        parsb.Length = 0;

        var parametroInt = 0;
        var negativo = false;

        c = rtf.Read();

        if (!Char.IsLetter((char)c))
        {
            if (c == '\\' || c == '{' || c == '}')   
            {
                token.Type = RtfTokenType.Text;
                token.Key = ((char)c).ToString();
            }
            else     
            {
                token.Type = RtfTokenType.Control;
                token.Key = ((char)c).ToString();

                if (token.Key == "\'")
                {
                    var cod = "";

                    cod += (char)rtf.Read();
                    cod += (char)rtf.Read();

                    token.HasParameter = true;

                    token.Parameter = Convert.ToInt32(cod, 16);
                }

            }

            c = rtf.Read();
        }
        else       
        {
            while (Char.IsLetter((char)c))
            {
                keysb.Append((char)c);

                c = rtf.Read();
            }

            token.Type = RtfTokenType.Keyword;
            token.Key = keysb.ToString();

            if (Char.IsDigit((char)c) || c == '-')
            {
                token.HasParameter = true;

                if (c == '-')
                {
                    negativo = true;

                    c = rtf.Read();
                }

                while (Char.IsDigit((char)c))
                {
                    parsb.Append((char)c);

                    c = rtf.Read();
                }

                parametroInt = Convert.ToInt32(parsb.ToString());

                if (negativo)
                    parametroInt = -parametroInt;

                token.Parameter = parametroInt;
            }

            if (c == ' ')
            {
                c = rtf.Read();
            }
        }
    }

    private void ParseText(RtfToken token)
    {
        keysb.Length = 0;

        while (c != '\\' && c != '}' && c != '{' && c != Eof)
        {
            keysb.Append((char)c);

            c = rtf.Read();

            while (c == '\r' || c == '\n' || c == '\t' || c == '\0')
                c = rtf.Read();
        }

        token.Key = keysb.ToString();
    }

}