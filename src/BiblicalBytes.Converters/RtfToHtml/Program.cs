using RtfToHtml;

namespace BiblicalBytes.Converters.RtfToHtml;

internal class Program
{
    [STAThread()]
    private static void Main(string[] args)

    {
        var htmlofExample = File.ReadAllText("html.html");
        var rtfofExample = File.ReadAllText("rtf.rtf");

        // Console.WriteLine(htmlofExample);
        var htmlToRtf = new Rtf();
        SaveToRtfFile(htmlToRtf.ConvertHtmlToRtf(htmlofExample));
        var rtfToHtml = new Html();
        SaveToHtmlFile(rtfToHtml.ConvertRtfToHtml(rtfofExample));
    }

    private static void SaveToRtfFile( string html)
    {
        //Console.WriteLine(html);

        // Assume we already have a document 'dc'.
        File.WriteAllText(@"../../Rtf.rtf", html);
    }

    private static void SaveToHtmlFile(string rtf)
    {
        // Assume we already have a document 'dc'.
        File.WriteAllText(@"../../html1.html", rtf);
    }


}