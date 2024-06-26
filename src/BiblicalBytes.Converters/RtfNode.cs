using TreeCollections;

namespace BiblicalBytes.Converters;

public class RtfNode : SerialTreeNode<RtfNode>
{
    public NodeType NodeType { get; set; } = NodeType.None;

    public Token Token { get; set; } = new Token();
}