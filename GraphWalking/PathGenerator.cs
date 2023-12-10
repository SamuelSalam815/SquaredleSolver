using GraphWalking.Graphs;

namespace GraphWalking;

/// <summary>
///     Generates paths from <see cref="AdjacencyList{TNodeId}"/>s.
///     In a path a node may never be visited more than once.
/// </summary>
public class PathGenerator<TNodeId> where TNodeId : notnull, IEquatable<TNodeId>
{
    private readonly AdjacencyList<TNodeId> adjacencyList;

    public PathGenerator(AdjacencyList<TNodeId> adjacencyList)
    {
        this.adjacencyList = adjacencyList;
    }

    public IEnumerable<List<TNodeId>> Generate()
    {
        foreach (var node in adjacencyList.GetAllNodes())
        {
            foreach (var path in GenerateStartingFrom(node))
            {
                yield return path;
            }
        }
    }

    private IEnumerable<List<TNodeId>> GenerateStartingFrom(TNodeId node)
    {
        Queue<List<TNodeId>> expandingPaths = new();
        expandingPaths.Enqueue(new List<TNodeId>() { node });

        while (expandingPaths.Count > 0)
        {
            List<TNodeId> currentPath = expandingPaths.Dequeue();
            yield return currentPath;
            expandingPaths.EnqueueAll(GetExtendedPaths(currentPath));
        }
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

    public IEnumerable<List<TNodeId>> FastGenerate()
    {
        // Perform a depth-first traversal, starting at each node in the graph
        List<TNodeId> emptyList = new();
        foreach (var startingNode in adjacencyList.GetAllNodes())
        {
            List<TNodeId> currentPath = new();
            Stack<CheckPoint> stack = new();
            stack.Push(new CheckPoint(startingNode, default));

            while (stack.Count > 0)
            {
                // revert the current path to the top most checkpoint
                (TNodeId currentNode, TNodeId? previousNode) = stack.Pop();
                while (currentPath.Count > 0 && !currentPath.Last().Equals(previousNode))
                {
                    currentPath.RemoveAt(currentPath.Count - 1);
                }

                currentPath.Add(currentNode);
                bool pathAtMaximumLength = true;
                foreach (var nextNode in adjacencyList.GetValueOrDefault(currentNode, emptyList))
                {
                    if (!currentPath.Contains(nextNode))
                    {
                        pathAtMaximumLength = false;
                        stack.Push(new CheckPoint(nextNode, currentNode));
                    }
                }

                yield return new List<TNodeId>(currentPath);
                if (pathAtMaximumLength)
                {
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }
    }

    private record struct CheckPoint(TNodeId node, TNodeId? previousNode);
}