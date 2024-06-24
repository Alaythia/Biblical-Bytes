namespace BiblicalBytes.Converters.RtfToHtml;

internal static class Alignment
{
    public static AlignmentAttribute[] AlignmentReferenceList =
    [
        new AlignmentAttribute()
            { Name= "center",   Reference="\\qc" },
        new AlignmentAttribute()

            { Name="left",     Reference="\\ql" },
        new AlignmentAttribute()

            { Name="right",    Reference="\\qr" },
        new AlignmentAttribute()

            { Name="justify",  Reference="\\qj" }
    ];

    public static string GetRtfAlignmentReference(string propertyName)
    {
        var alignmentReference = "";
        foreach(var element in AlignmentReferenceList)
        {
            if (element.Name == propertyName.Trim())
            {
                alignmentReference = element.Reference;        
            }
        }
           
        return alignmentReference;
    }
}