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

    public List<Path<TNodeId>> Generate()
    {
        List<Path<TNodeId>> allPaths = new();

        foreach (var node in adjacencyList.GetAllNodes())
        {
            allPaths.AddRange(GenerateStartingFrom(node));
        }

        return allPaths;
    }

    private List<Path<TNodeId>> GenerateStartingFrom(TNodeId node)
    {
        List<Path<TNodeId>> pathsGenerated = new();
        Queue<Path<TNodeId>> expandingPaths = new();
        expandingPaths.Enqueue(new Path<TNodeId>() { node });

        while (expandingPaths.Count > 0)
        {
            Path<TNodeId> currentPath = expandingPaths.Dequeue();
            pathsGenerated.Add(currentPath);
            expandingPaths.EnqueueAll(GetExtendedPaths(currentPath));
        }

        return pathsGenerated;
    }

    private IEnumerable<Path<TNodeId>> GetExtendedPaths(Path<TNodeId> path)
    {
        TNodeId lastNode = path.Last();
        if (!adjacencyList.ContainsKey(lastNode))
        {
            return new List<Path<TNodeId>>();
        }

        List<Path<TNodeId>> extendedPaths = new();
        var candidateNodes = adjacencyList[lastNode].Where(node => !path.Contains(node));
        foreach (var node in candidateNodes)
        {
            Path<TNodeId> nextPath = new(path)
            {
                node
            };
            extendedPaths.Add(nextPath);
        }

        return extendedPaths;
    }
}