using System.Collections.Generic;

namespace TreeCollections.Tree.ItemTree.EntityTree.MutableEntityTree.OrderComparers;

internal class NodeOrderItemComparer<TNode, TItem> : IComparer<TNode> 
    where TNode : ItemTreeNode<TNode, TItem>
{
    private readonly IComparer<TItem> itemComparer;

    public NodeOrderItemComparer(IComparer<TItem> itemComparer)
    {
            this.itemComparer = itemComparer;
        }
        
    public int Compare(TNode x, TNode y)
    {
            return itemComparer.Compare(x.Item, y.Item);
        }
}