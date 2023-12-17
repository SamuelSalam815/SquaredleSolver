namespace GraphWalking.Graphs;

/// <summary>
///     Represents a graph.
/// </summary>
/// <typeparam name="TNode">The type of the unique identifier for a node.</typeparam>
public class AdjacencyList<TNode> : Dictionary<TNode, List<TNode>> where TNode : notnull
{

    public List<TNode> GetAllNodes() => this.Aggregate(new HashSet<TNode>(), CollectUniqueNodes).ToList();

    public List<TNode> GetBranchNodes() => Keys.Where(key => this[key].Count > 0).ToList();

    public List<TNode> GetLeafNodes() => GetAllNodes().Where(node => !ContainsKey(node)).ToList();

    /// <summary>
    ///     Aggregates a collection of unique nodes.
    /// </summary>
    /// <param name="uniqueNodes">The current set of unique nodes.</param>
    /// <param name="entry">The next entry to aggregate.</param>
    /// <returns><paramref name="uniqueNodes"/></returns>
    private static HashSet<TNode> CollectUniqueNodes(HashSet<TNode> uniqueNodes, KeyValuePair<TNode, List<TNode>> entry)
    {
        (TNode node, List<TNode> adjacentNodes) = entry;
        uniqueNodes.Add(node);
        foreach (TNode nodeId in adjacentNodes)
        {
            uniqueNodes.Add(nodeId);
        }
        return uniqueNodes;
    }

    public override string ToString()
    {
        static string StringifyAdjacencies(List<TNode> adjacencies)
        {
            return $"[{string.Join(", ", adjacencies)}]";
        }

        static string StringifyEntry(KeyValuePair<TNode, List<TNode>> entry)
        {
            return $"{entry.Key}: {StringifyAdjacencies(entry.Value)}";
        }

        return "{" + string.Join(", ", this.Select(StringifyEntry)) + "}";
    }
}