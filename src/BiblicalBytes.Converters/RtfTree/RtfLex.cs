using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

/// <summary>
/// Provides lexical analysis for RTF text, converting it into a stream of tokens for further processing.
/// </summary>
public class RtfLex
{
    private readonly TextReader rtf;

    private readonly StringBuilder keysb;

    private readonly StringBuilder parsb;

    private int c;

    private const int Eof = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="RtfLex"/> class with the specified RTF text reader.
    /// </summary>
    /// <param name="rtfReader">The text reader containing the RTF content to be analyzed.</param>
    public RtfLex(TextReader rtfReader)
    {
        rtf = rtfReader;

        keysb = new StringBuilder();
        parsb = new StringBuilder();

        c = rtf.Read();
    }

    /// <summary>
    /// Reads the next token from the RTF content.
    /// </summary>
    /// <returns>The next <see cref="RtfToken"/> found in the RTF content.</returns>
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

    /// <summary>
    /// Parses a keyword from the RTF content, updating the provided token with the keyword details.
    /// </summary>
    /// <param name="token">The token to be updated with keyword details.</param>
    private void ParseKeyword(RtfToken token)
    {
        keysb.Clear();
        parsb.Clear();

        int parametroInt;
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

    /// <summary>
    /// Parses text from the RTF content, updating the provided token with the text details.
    /// </summary>
    /// <param name="token">The token to be updated with text details.</param>
    private void ParseText(RtfToken token)
    {
        keysb.Clear();

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