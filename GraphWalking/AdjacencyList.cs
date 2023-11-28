namespace GraphWalking;

/// <summary>
///     Represents a graph.
/// </summary>
/// <typeparam name="TNodeId">The type of the unique identifier for a node.</typeparam>
public class AdjacencyList<TNodeId> : Dictionary<TNodeId, List<TNodeId>> where TNodeId : notnull
{

    public List<TNodeId> GetAllNodes() => this.Aggregate(new HashSet<TNodeId>(), CollectUniqueNodes).ToList();

    public List<TNodeId> GetBranchNodes() => Keys.Where(key => this[key].Count > 0).ToList();

    public List<TNodeId> GetLeafNodes() => GetAllNodes().Where(node => !ContainsKey(node)).ToList();

    /// <summary>
    ///     Aggregates a collection of unique nodes.
    /// </summary>
    /// <param name="uniqueNodes">The current set of unique nodes.</param>
    /// <param name="entry">The next entry to aggregate.</param>
    /// <returns><paramref name="uniqueNodes"/></returns>
    private static HashSet<TNodeId> CollectUniqueNodes(HashSet<TNodeId> uniqueNodes, KeyValuePair<TNodeId, List<TNodeId>> entry)
    {
        (TNodeId node, List<TNodeId> adjacentNodes) = entry;
        uniqueNodes.Add(node);
        foreach (TNodeId nodeId in adjacentNodes)
        {
            uniqueNodes.Add(nodeId);
        }
        return uniqueNodes;
    }

    public override string ToString()
    {
        static string StringifyAdjacencies(List<TNodeId> adjacencies)
        {
            return $"[{string.Join(", ", adjacencies)}]";
        }

        static string StringifyEntry(KeyValuePair<TNodeId, List<TNodeId>> entry)
        {
            return $"{entry.Key}: {StringifyAdjacencies(entry.Value)}";
        }

        return "{" + string.Join(", ", this.Select(StringifyEntry)) + "}";
    }
}