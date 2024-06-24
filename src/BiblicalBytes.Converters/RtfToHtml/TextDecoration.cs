namespace BiblicalBytes.Converters.RtfToHtml;

public static class TextDecoration
{
    public static DecorationAttribute[] TextDecorationReferenceList =
    [
        new DecorationAttribute()
            { Name= "underline",   Reference="\\ul" },
        new DecorationAttribute()

            { Name="overline",     Reference="\\ol" },
        new DecorationAttribute()

            { Name="line-through",    Reference="\\strike" }
    ];
    public static string GetRtfTextDecorationReference(string propertyName)
    {
        var alignmentReference = "";
        foreach (var element in TextDecorationReferenceList)
        {
            if (element.Name == propertyName.Trim())
            {
                alignmentReference = element.Reference;
            }
        }

        return alignmentReference;
    }
}