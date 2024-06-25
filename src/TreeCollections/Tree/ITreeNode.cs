namespace TreeCollections.Tree;

/// <summary>
/// Represents a tree node with a unique hierarchy identity
/// </summary>
public interface ITreeNode
{
    HierarchyPosition.HierarchyPosition HierarchyId { get; }
}