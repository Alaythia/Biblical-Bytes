namespace BiblicalBytes.Converters;

/// <summary>
/// Defines the types of tokens that can be identified in an RTF document.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// Represents an undefined token type.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents a keyword token.
    /// </summary>
    Keyword = 1,

    /// <summary>
    /// Represents a control token.
    /// </summary>
    Control = 2,

    /// <summary>
    /// Represents a text token.
    /// </summary>
    Text = 3,

    /// <summary>
    /// Represents the end of file token.
    /// </summary>
    Eof = 4,

    /// <summary>
    /// Represents the start of a group token.
    /// </summary>
    GroupStart = 5,

    /// <summary>
    /// Represents the end of a group token.
    /// </summary>
    GroupEnd = 6
}