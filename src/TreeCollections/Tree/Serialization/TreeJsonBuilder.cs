using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeCollections.InternalUtilities;

namespace TreeCollections.Tree.Serialization;

/// <summary>
/// Builder for representing a tree as JSON
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class TreeJsonBuilder<TNode>(
    Func<TNode, Dictionary<string, string>> toProperties,
    string childrenPropertyName = "Children") where TNode : TreeNode<TNode>
{
    private Func<TNode, bool> allowNext;
    private int maxRelativeDepth;
    private StringBuilder builder;

    public TreeJsonBuilder(string childrenPropertyName = "Children") 
        : this(n => new Dictionary<string, string> { {"HierarchyId", n.HierarchyId.ToString("/").WrapDoubleQuotes()}}, childrenPropertyName)
    { }

    /// <summary>
    /// Build JSON representation of tree from given node
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="includeRoot">Include relative root in JSON</param>
    /// <returns></returns>
    public string ToJson(TNode node, bool includeRoot = true)
    {
            return ToJson(node, n => true, int.MaxValue, includeRoot);
        }

    /// <summary>
    /// Build JSON representation of tree from given node with a filtering predicate
    /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
    /// <param name="includeRoot">Include relative root in JSON</param>
    /// <returns></returns>
    public string ToJson(TNode node, Func<TNode, bool> allowNext, bool includeRoot = true)
    {
            return ToJson(node, allowNext, int.MaxValue, includeRoot);
        }

    /// <summary>
    /// Build JSON representation of tree from given node to maximum relative depth
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
    /// <param name="includeRoot">Include relative root in JSON</param>
    /// <returns></returns>
    public string ToJson(TNode node, int maxRelativeDepth, bool includeRoot = true)
    {
            return ToJson(node, n => true, maxRelativeDepth, includeRoot);
        }

    /// <summary>
    /// Build JSON representation of tree from given node with a filtering predicate to maximum relative depth.
    /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
    /// </summary>
    /// <param name="root">Relative root node</param>
    /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
    /// <param name="includeRoot">Include relative root in JSON</param>
    /// <returns></returns>
    public string ToJson(TNode root, Func<TNode, bool> allowNext, int maxRelativeDepth, bool includeRoot = true)
    {
            if (!allowNext(root) || maxRelativeDepth < 0) return string.Empty;

            this.allowNext = allowNext;
            this.maxRelativeDepth = maxRelativeDepth;
            builder = new StringBuilder();

            if (includeRoot)
            {
                BuildItem(root, 0);
            }
            else
            {
                BuildChildren(root, 0, string.Empty);
            }

            return builder.ToString();
        }

    private void BuildItem(TNode node, int curDepth)
    {
            builder.Append("{");

            var propertyMap = toProperties(node);

            var hasProperties = propertyMap.Count > 0;

            if (hasProperties)
            {
                builder.Append(propertyMap.Select(kvp => $"{kvp.Key.WrapDoubleQuotes()}:{kvp.Value}").ToCsv());
            }

            BuildChildren(node, curDepth, $"{(hasProperties ? "," : string.Empty)}{childrenPropertyName.WrapDoubleQuotes()}:");

            builder.Append("}");
        }

    private void BuildChildren(TNode node, int curDepth, string prefix)
    {
            if (curDepth++ == maxRelativeDepth) return;

            var effectiveChildren = node.Children.Where(allowNext).ToArray();

            if (effectiveChildren.Length == 0) return;

            builder.Append(prefix);
            builder.Append("[");

            foreach (var child in effectiveChildren)
            {
                BuildItem(child, curDepth);

                if (child.NextSibling != null)
                {
                    builder.Append(",");
                }
            }

            builder.Append("]");
        }
}