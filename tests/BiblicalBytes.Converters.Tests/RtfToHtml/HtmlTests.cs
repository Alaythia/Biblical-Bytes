using BiblicalBytes.Converters.RtfToHtml;

namespace BiblicalBytes.Converters.Tests.RtfToHtml;

public class HtmlTests
{
    [Fact]
    public void TestEmptyRtfString()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml("");
        Assert.Equal("<!DOCTYPE html><html><body> </body></html>", result);
    }

    [Fact]
    public void TestNullRtfString()
    {
        var converter = new Html();
        Assert.Throws<ArgumentNullException>(() => converter.ConvertRtfToHtml(null));
    }

    [Fact]
    public void TestSimpleTextRtf()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi This is a test.}");
        Assert.Contains("This is a test.", result);
    }

    [Fact]
    public void TestRtfWithBoldText()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi\b This is bold\b0 }");
        Assert.Contains("<strong>This is bold</strong>", result);
    }

    [Fact]
    public void TestRtfWithItalicText()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi\i This is italic\i0 }");
        Assert.Contains("<em>This is italic</em>", result);
    }

    [Fact]
    public void TestRtfWithHyperlinks()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi This is a {\field{\*\fldinst{HYPERLINK ""http://example.com""}}{\fldrslt{link}}}.}");
        Assert.Contains("<a href=\"http://example.com\">link</a>", result);
    }

    [Fact]
    public void TestRtfWithUnsupportedKeywords()
    {
        var converter = new Html();
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi\shad This is text with unsupported keyword.}");
        // Assuming unsupported keywords are ignored, the text should still be present.
        Assert.Contains("This is text with unsupported keyword.", result);
    }

    [Fact]
    public void TestAutoParagraphEnabled()
    {
        var converter = new Html { AutoParagraph = true };
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi This is a paragraph.\par\par This is another paragraph.}");
        Assert.Contains("<p>This is a paragraph.</p><p>This is another paragraph.</p>", result);
    }

    [Fact]
    public void TestEscapeHtmlEntities()
    {
        var converter = new Html { EscapeHtmlEntities = true };
        var result = converter.ConvertRtfToHtml(@"{\rtf1\ansi This is <b>bold</b> text.}");
        // Assuming HtmlEntities.Encode method correctly escapes HTML entities.
        Assert.Contains("This is &lt;b&gt;bold&lt;/b&gt; text.", result);
    }
}