namespace BiblicalBytes.Converters;

/// <summary>
/// Represents a token in the RTF structure.
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or sets the type of the token.
    /// </summary>
    public TokenType Type { get; set; } = TokenType.None;

    /// <summary>
    /// Gets or sets the key of the token. This could be a control word or symbol.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this token has a parameter.
    /// </summary>
    public bool HasParameter { get; set; }

    /// <summary>
    /// Gets or sets the parameter of the token, if any.
    /// </summary>
    public int Parameter { get; set; }
}