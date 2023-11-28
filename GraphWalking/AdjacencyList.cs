namespace GraphWalking;

/// <summary>
///     Represents a graph.
/// </summary>
/// <typeparam name="TNodeId">The type of the unique identifier for a node.</typeparam>
public class AdjacencyList<TNodeId> : Dictionary<TNodeId, List<TNodeId>> where TNodeId : notnull
{
    /// <summary>
    ///     Gets all nodes referred to in this <see cref="AdjacencyList{TNodeId}"/>.
    /// </summary>
    public List<TNodeId> AllNodes
    {
        get
        {
            return this.Aggregate(new HashSet<TNodeId>(), CollectUniqueNodes).ToList();
        }
    }

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

}
