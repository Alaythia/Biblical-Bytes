using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeCollections.InternalUtilities;

namespace TreeCollections.Tree.Serialization;

/// <summary>
/// Builder for representing a tree as HTML
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class TreeHtmlBuilder<TNode> where TNode : TreeNode<TNode>
{
    private readonly HtmlBuildDefinition<TNode> def;

    private Func<TNode, bool> allowNext;
    private int maxRelativeDepth;
    private StringBuilder builder;

    public TreeHtmlBuilder(HtmlBuildDefinition<TNode> def = null)
    {
            this.def = def ?? new HtmlBuildDefinition<TNode>();
        }

    /// <summary>
    /// Build HTML representation of tree from given node
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="includeRoot">Include relative root in HTML</param>
    /// <returns></returns>
    public string ToHtml(TNode node, bool includeRoot = true)
    {
            return ToHtml(node, n => true, int.MaxValue, includeRoot);
        }

    /// <summary>
    /// Build HTML representation of tree from given node with a filtering predicate
    /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
    /// <param name="includeRoot">Include relative root in HTML</param>
    /// <returns></returns>
    public string ToHtml(TNode node, Func<TNode, bool> allowNext, bool includeRoot = true)
    {
            return ToHtml(node, allowNext, int.MaxValue, includeRoot);
        }

    /// <summary>
    /// Build HTML representation of tree from given node to maximum relative depth
    /// </summary>
    /// <param name="node">Relative root node</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
    /// <param name="includeRoot">Include relative root in HTML</param>
    /// <returns></returns>
    public string ToHtml(TNode node, int maxRelativeDepth, bool includeRoot = true)
    {
            return ToHtml(node, n => true, maxRelativeDepth, includeRoot);
        }

    /// <summary>
    /// Build HTML representation of tree from given node with a filtering predicate to maximum relative depth.
    /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
    /// </summary>
    /// <param name="root">Relative root node</param>
    /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
    /// <param name="includeRoot">Include relative root in HTML</param>
    /// <returns></returns>
    public string ToHtml(TNode root, Func<TNode, bool> allowNext, int maxRelativeDepth, bool includeRoot = true)
    {
            if (!allowNext(root) || maxRelativeDepth < 0) return string.Empty;

            this.allowNext = allowNext;
            this.maxRelativeDepth = maxRelativeDepth;
            builder = new StringBuilder();

            if (includeRoot)
            {
                builder.Append($"<{def.RootElementName}{SerializeAttributes(def.GetRootAttributes(root))}>");
                builder.Append(def.GetRootPreHtml(root));

                BuildItem(root, 0);

                builder.Append(def.GetRootPostHtml(root));
                builder.Append($"</{def.RootElementName}>");
            }
            else
            {
                BuildChildren(root, 0);
            }
            
            return builder.ToString();
        }

    private void BuildItem(TNode node, int curDepth)
    {
            builder.Append($"<{def.ItemElementName}{SerializeAttributes(def.GetItemAttributes(node))}>");
            builder.Append(def.GetItemPreHtml(node));

            if (node.Children.Count != 0)
            {
                BuildChildren(node, curDepth);
            }
            
            builder.Append(def.GetItemPostHtml(node));
            builder.Append($"</{def.ItemElementName}>");
        }

    private void BuildChildren(TNode node, int curDepth)
    {
            if (curDepth++ == maxRelativeDepth) return;

            builder.Append($"<{def.ContainerElementName}{SerializeAttributes(def.GetContainerAttributes(node))}>");
            builder.Append(def.GetContainerPreHtml(node));

            foreach (var child in node.Children.Where(allowNext))
            {
                BuildItem(child, curDepth);
            }

            builder.Append(def.GetContainerPostHtml(node));
            builder.Append($"</{def.ContainerElementName}>");
        }

    private static string SerializeAttributes(IDictionary<string, string> attributes)
    {
            return attributes.Count != 0 
                ? " " + attributes.Select(kvp => $"{kvp.Key}={kvp.Value.WrapDoubleQuotes()}").SerializeToString(" ") 
                : string.Empty;
        }
}