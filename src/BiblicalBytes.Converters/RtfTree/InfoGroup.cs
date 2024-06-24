using System.Text;

namespace BiblicalBytes.Converters.RtfTree;

public class InfoGroup
{
    public string Title { get; set; } = "";

    public string Subject { get; set; } = "";

    public string Author { get; set; } = "";

    public string Manager { get; set; } = "";

    public string Company { get; set; } = "";

    public string Operator { get; set; } = "";

    public string Category { get; set; } = "";

    public string Keywords { get; set; } = "";

    public string Comment { get; set; } = "";

    public string DocComment { get; set; } = "";

    public string HlinkBase { get; set; } = "";

    public DateTime CreationTime { get; set; } = DateTime.MinValue;

    public DateTime RevisionTime { get; set; } = DateTime.MinValue;

    public DateTime LastPrintTime { get; set; } = DateTime.MinValue;

    public DateTime BackupTime { get; set; } = DateTime.MinValue;

    public int Version { get; set; } = -1;

    public int InternalVersion { get; set; } = -1;

    public int EditingTime { get; set; } = -1;

    public int NumberOfPages { get; set; } = -1;

    public int NumberOfWords { get; set; } = -1;

    public int NumberOfChars { get; set; } = -1;

    public int Id { get; set; } = -1;

    public override string ToString()
    {
        var str = new StringBuilder();

        str.AppendLine("Title     : " + Title);
        str.AppendLine("Subject   : " + Subject);
        str.AppendLine("Author    : " + Author);
        str.AppendLine("Manager   : " + Manager);
        str.AppendLine("Company   : " + Company);
        str.AppendLine("Operator  : " + Operator);
        str.AppendLine("Category  : " + Category);
        str.AppendLine("Keywords  : " + Keywords);
        str.AppendLine("Comment   : " + Comment);
        str.AppendLine("DComment  : " + DocComment);
        str.AppendLine("HLinkBase : " + HlinkBase);
        str.AppendLine("Created   : " + CreationTime);
        str.AppendLine("Revised   : " + RevisionTime);
        str.AppendLine("Printed   : " + LastPrintTime);
        str.AppendLine("Backup    : " + BackupTime);
        str.AppendLine("Version   : " + Version);
        str.AppendLine("IVersion  : " + InternalVersion);
        str.AppendLine("Editing   : " + EditingTime);
        str.AppendLine("Num Pages : " + NumberOfPages);
        str.AppendLine("Num Words : " + NumberOfWords);
        str.AppendLine("Num Chars : " + NumberOfChars);
        str.AppendLine("Id        : " + Id);

        return str.ToString();
    }
}