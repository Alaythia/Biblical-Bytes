namespace BiblicalBytes.Bible.Models;

public interface IChapter
{
    byte Number { get; }
    IReadOnlyList<IVerse> Verses { get; }
}