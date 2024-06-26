using System.Collections;
using System.Collections.Generic;

namespace TreeCollections.Tree.Enumeration;

/// <summary>
/// Container exposing enumeration with an injectable enumerator
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class EnumeratorHost<TNode> : IEnumerable<TNode>
    where TNode : TreeNode<TNode>
{
    private readonly IEnumerator<TNode> enumerator;

    internal EnumeratorHost(IEnumerator<TNode> enumerator)
    {
            this.enumerator = enumerator;
        }
  
    public IEnumerator<TNode> GetEnumerator()
    {
            return enumerator;
        }

    IEnumerator IEnumerable.GetEnumerator()
    {
            return GetEnumerator();
        }
}