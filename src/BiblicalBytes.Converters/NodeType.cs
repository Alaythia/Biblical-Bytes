namespace BiblicalBytes.Converters;

/// <summary>
/// Represents the type of RTF node.
/// </summary>
public enum NodeType
{
    /// <summary>
    /// The root of the RTF document tree.
    /// </summary>
    Root = 0,

    /// <summary>
    /// A keyword in the RTF content.
    /// </summary>
    Keyword = 1,

    /// <summary>
    /// A control word in the RTF content.
    /// </summary>
    Control = 2,

    /// <summary>
    /// Text content within the RTF document.
    /// </summary>
    Text = 3,

    /// <summary>
    /// A group of RTF nodes, enclosed in braces.
    /// </summary>
    Group = 4,

    /// <summary>
    /// Represents an undefined or uninitialized node.
    /// </summary>
    None = 5
}