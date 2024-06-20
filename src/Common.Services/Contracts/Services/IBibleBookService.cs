namespace Common.Contracts.Services;

public interface IBibleBookService
{
    /// <summary>Gets the Bible book names in the specified language.</summary>
    /// <param name="language">The language.</param>
    /// <returns>The Bible book names in the specified language.</returns>
    IReadOnlyList<string> GetBookNames(string language);

    /// <summary>Gets the Bible book names in the specified language.</summary>
    /// <param name="language">The language.</param>
    /// <param name="translation">The translation.</param>
    /// <returns>The Bible book names in the specified language.</returns>
    IReadOnlyList<string> GetBookNames(string language, string translation);
}