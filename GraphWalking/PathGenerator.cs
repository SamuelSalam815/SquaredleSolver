using GraphWalking.Graphs;

namespace GraphWalking;

/// <summary>
///     Generates paths from <see cref="AdjacencyList{TNodeId}"/>s.
///     In a path a node may never be visited more than once.
/// </summary>
public class PathGenerator<TNodeId> where TNodeId : notnull
{
    private readonly AdjacencyList<TNodeId> adjacencyList;

    public PathGenerator(AdjacencyList<TNodeId> adjacencyList)
    {
        this.adjacencyList = adjacencyList;
    }

    public List<List<TNodeId>> Generate()
    {
        List<List<TNodeId>> allPaths = new();

        foreach (var node in adjacencyList.GetAllNodes())
        {
            allPaths.AddRange(GenerateStartingFrom(node));
        }

        return allPaths;
    }

    private List<List<TNodeId>> GenerateStartingFrom(TNodeId node)
    {
        List<List<TNodeId>> pathsGenerated = new();
        Queue<List<TNodeId>> expandingPaths = new();
        expandingPaths.Enqueue(new List<TNodeId>() { node });

        while (expandingPaths.Count > 0)
        {
            List<TNodeId> currentPath = expandingPaths.Dequeue();
            pathsGenerated.Add(currentPath);
            expandingPaths.EnqueueAll(GetExtendedPaths(currentPath));
        }

        return pathsGenerated;
    }

    private IEnumerable<List<TNodeId>> GetExtendedPaths(List<TNodeId> path)
    {
        TNodeId lastNode = path.Last();
        if (!adjacencyList.ContainsKey(lastNode))
        {
            return new List<List<TNodeId>>();
        }

        var candidateNodes = adjacencyList[lastNode].Where(node => !path.Contains(node));
        return candidateNodes.Select(node => new List<TNodeId>(path) { node });
    }
}