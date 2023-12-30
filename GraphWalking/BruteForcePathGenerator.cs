using GraphWalking.Graphs;

namespace GraphWalking;

/// <summary>
///     Generates paths from <see cref="AdjacencyList{TNodeId}"/>s.
///     In a path a node may never be visited more than once.
/// </summary>
public static class BruteForcePathGenerator<TNode> where TNode : IEquatable<TNode>
{
    private record struct CheckPoint(TNode Node, TNode PreviousNode);

    /// <summary>
    ///     Finds all paths from the starting node to every node in the adjacency list.
    /// </summary>
    public static IEnumerable<List<TNode>> EnumerateAllPaths(
        TNode startingNode,
        AdjacencyList<TNode> adjacencyList)
    {
        List<TNode> emptyList = new();
        List<TNode> currentPath = new();
        Stack<CheckPoint> stack = new();
        stack.Push(new CheckPoint(startingNode, startingNode));

        while (stack.Count > 0)
        {
            // revert the current path to the top most checkpoint
            (TNode currentNode, TNode previousNode) = stack.Pop();
            while (currentPath.Count > 0 && !currentPath.Last().Equals(previousNode))
            {
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            currentPath.Add(currentNode);
            foreach (TNode nextNode in adjacencyList.GetValueOrDefault(currentNode, emptyList))
            {
                if (!currentPath.Contains(nextNode))
                {
                    stack.Push(new CheckPoint(nextNode, currentNode));
                }
            }

            yield return new List<TNode>(currentPath);
        }
    }

    /// <summary>
    ///     Finds all paths between all pairs of nodes in the adjacency list.
    /// </summary>
    public static IEnumerable<List<TNode>> EnumerateAllPaths(AdjacencyList<TNode> adjacencyList)
    {
        foreach (TNode node in adjacencyList.GetAllNodes())
        {
            foreach (List<TNode> path in EnumerateAllPaths(node, adjacencyList))
            {
                yield return path;
            }
        }
    }
}