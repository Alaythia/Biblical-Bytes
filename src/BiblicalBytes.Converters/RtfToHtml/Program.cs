namespace BiblicalBytes.Converters.RtfToHtml;

public class Program
{
    [STAThread()]
    private static void Main(string[] args)

    {
        var htmlOfExample = File.ReadAllText("html.html");
        var rtfOfExample = File.ReadAllText("rtf.rtf");

        var htmlToRtf = new Rtf();
        SaveToRtfFile(htmlToRtf.ConvertHtmlToRtf(htmlOfExample));
        var rtfToHtml = new Html();
        SaveToHtmlFile(rtfToHtml.ConvertRtfToHtml(rtfOfExample));
    }

    private static void SaveToRtfFile( string html)
    {
        // Assume we already have a document 'dc'.
        File.WriteAllText(@"../../Rtf.rtf", html);
    }

    private static void SaveToHtmlFile(string rtf)
    {
        // Assume we already have a document 'dc'.
        File.WriteAllText(@"../../html1.html", rtf);
    }


}