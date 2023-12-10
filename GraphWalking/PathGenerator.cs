using GraphWalking.Graphs;
using System.Collections.Concurrent;

namespace GraphWalking;

/// <summary>
///     Generates paths from <see cref="AdjacencyList{TNodeId}"/>s.
///     In a path a node may never be visited more than once.
/// </summary>
public class PathGenerator<TNodeId> where TNodeId : notnull, IEquatable<TNodeId>
{
    private readonly AdjacencyList<TNodeId> adjacencyList;
    private static readonly List<TNodeId> EmptyList = new();
    private record struct CheckPoint(TNodeId Node, TNodeId PreviousNode);

    public PathGenerator(AdjacencyList<TNodeId> adjacencyList)
    {
        this.adjacencyList = adjacencyList;
    }

    public IEnumerable<List<TNodeId>> Generate()
    {
        // Perform a depth-first traversal, starting at each node in the graph
        foreach (TNodeId startingNode in adjacencyList.GetAllNodes())
        {
            foreach (List<TNodeId> path in GeneratePathsStartingWith(startingNode))
            {
                yield return path;
            }
        }
    }

    public IEnumerable<List<TNodeId>> ParallelGenerate()
    {
        ConcurrentBag<IEnumerable<List<TNodeId>>> resultSynchronizer = new();
        Parallel.ForEach(adjacencyList.GetAllNodes(), startingNode => resultSynchronizer.Add(GeneratePathsStartingWith(startingNode)));
        foreach (IEnumerable<List<TNodeId>> pathCollection in resultSynchronizer)
        {
            foreach (List<TNodeId> path in pathCollection)
            {
                yield return path;
            }
        }
    }

    private IEnumerable<List<TNodeId>> GeneratePathsStartingWith(TNodeId startingNode)
    {
        List<TNodeId> currentPath = new();
        Stack<CheckPoint> stack = new();
        stack.Push(new CheckPoint(startingNode, startingNode));

        while (stack.Count > 0)
        {
            // revert the current path to the top most checkpoint
            (TNodeId currentNode, TNodeId previousNode) = stack.Pop();
            while (currentPath.Count > 0 && !currentPath.Last().Equals(previousNode))
            {
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            currentPath.Add(currentNode);
            bool pathAtMaximumLength = true;
            foreach (TNodeId nextNode in adjacencyList.GetValueOrDefault(currentNode, EmptyList))
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