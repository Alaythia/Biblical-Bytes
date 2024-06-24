namespace BiblicalBytes.Bible.Models;

public interface IBook
{
    byte Id { get; }
    string Name { get; }
    string Abbreviation { get; }
    IReadOnlyList<IChapter> Chapters { get; }
}