namespace BiblicalBytes.Bible.Models;

public interface IGreekWord
{
    // todo: still need to work out the details of this interface
    // the idea is to have a way to represent the original Greek and Hebrew words
    // in the Bible text
    string Number { get; }
    string Root { get; }
    string Definition { get; }
    bool IsPunctuation { get; }
    bool IsWhiteSpace { get; }
}