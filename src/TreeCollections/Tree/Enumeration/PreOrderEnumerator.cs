using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections.Tree.Enumeration;

/// <summary>
/// Enumerator for pre-order traversal with optional max depth of traversal
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class PreOrderEnumerator<TNode> : IEnumerator<TNode>
    where TNode : TreeNode<TNode>
{
    private readonly TNode rootOfIteration;
    private readonly int maxLevel;

    internal PreOrderEnumerator(TNode rootOfIteration, int? maxRelativeDepth = null)
    {
            this.rootOfIteration = rootOfIteration;
            maxLevel = rootOfIteration.Level + maxRelativeDepth ?? int.MaxValue;
            
            Current = null;
        } 
        
    public bool MoveNext()
    {
            if (Current == null)
            {
                Current = rootOfIteration;
                return true;
            }

            var firstChild = Current.Children.FirstOrDefault();

            if (firstChild != null && firstChild.Level <= maxLevel)
            {
                Current = firstChild;
                return true;
            }

            if (Current.Equals(rootOfIteration))
            {
                return false;
            }

            var node = Current;
            var nextSibling = Current.NextSibling;

            while (nextSibling == null)
            {
                node = node.Parent;

                if (node.Equals(rootOfIteration))
                {
                    return false;
                }

                nextSibling = node.NextSibling;
            }
            
            Current = nextSibling;
            return true;
        }

    public TNode Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose()
    { }

    public void Reset()
    {
            Current = null;
        }
}