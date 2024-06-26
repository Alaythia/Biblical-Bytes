using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections.Tree.Enumeration;

/// <summary>
/// Enumerator for pre-order traversal with a filtering predicate and optional max depth of traversal.
/// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class PreOrderFilteringEnumerator<TNode> : IEnumerator<TNode>
    where TNode : TreeNode<TNode>
{
    private readonly TNode rootOfIteration;
    private readonly int maxLevel;
    private readonly Func<TNode, bool> allowNext; 

    internal PreOrderFilteringEnumerator(TNode rootOfIteration, 
        Func<TNode, bool> allowNext, 
        int? maxRelativeDepth = null)
    {
            this.rootOfIteration = rootOfIteration;
            maxLevel = rootOfIteration.Level + maxRelativeDepth ?? int.MaxValue;
            this.allowNext = allowNext;
            
            Current = null;
        } 
        
    public bool MoveNext()
    {
            if (Current == null)
            {
                Current = rootOfIteration;
                return true;
            }

            var firstChild = Current.Children.FirstOrDefault(allowNext);

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

            var nextSibling = Current.SelectSiblingsAfter().FirstOrDefault(allowNext);

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