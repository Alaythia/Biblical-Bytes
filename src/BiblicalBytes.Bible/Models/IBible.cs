namespace BiblicalBytes.Bible.Models
{
    public interface IBible
    {
        string Version { get; }
        bool ContainsNewTestament { get; }
        bool ContainsOldTestament { get; }
        bool ContainsApocrypha { get; }
        IReadOnlyList<IBook> Books { get; }
    }
}
