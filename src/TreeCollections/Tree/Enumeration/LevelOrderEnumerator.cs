using System.Collections;
using System.Collections.Generic;

namespace TreeCollections.Tree.Enumeration;

/// <summary>
/// Enumerator for level-order (breadth-first) traversal with optional max depth of traversal
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class LevelOrderEnumerator<TNode> : IEnumerator<TNode>
    where TNode : TreeNode<TNode>
{
    private readonly TNode rootOfIteration;
    private readonly int maxDepth;
    private readonly Queue<TNode> queue;
    private int currentDepth;
    private int currentGenerationCount;
    private int nextGenerationCount;
        
    internal LevelOrderEnumerator(TNode rootOfIteration, int? maxRelativeDepth = null)
    {
            this.rootOfIteration = rootOfIteration;
            currentDepth = 0;
            queue = new Queue<TNode>();
            currentGenerationCount = 1;
            nextGenerationCount = 0;
            maxDepth = maxRelativeDepth ?? int.MaxValue;

            Current = null;
        }

    public bool MoveNext()
    {
            if (Current == null)
            {
                Current = rootOfIteration;
                ProcessCurrent();
                return true;
            }

            if (queue.Count == 0)
            {
                return false;
            }

            if (currentGenerationCount == 0)
            {
                SwapGeneration();
            }

            Current = queue.Dequeue();
            ProcessCurrent();

            return true;
        }

    private void ProcessCurrent()
    {
            currentGenerationCount--;

            if (currentDepth >= maxDepth) return;

            foreach (var child in Current.Children)
            {
                nextGenerationCount++;
                queue.Enqueue(child);
            }
        }

    private void SwapGeneration()
    {
            currentDepth++;
            currentGenerationCount = nextGenerationCount;
            nextGenerationCount = 0;
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