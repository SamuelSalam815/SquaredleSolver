using GraphWalking.Graphs;

namespace GraphWalking;

/// <summary>
///     Generates paths from <see cref="AdjacencyList{TNodeId}"/>s.
///     Attempts to optimize by avoiding paths known to be undesirable.
///     In a path a node may never be visited more than once.
/// </summary>
public class FailFastPathGenerator
{
    private readonly HashSet<string> validWords;
    private readonly HashSet<string> viablePrefixes = new();
    private readonly int minimumWordLength;

    public FailFastPathGenerator(HashSet<string> validWords, int minimumWordLength)
    {
        this.validWords = validWords;
        this.minimumWordLength = minimumWordLength;
    }

    private void Setup()
    {
        if (viablePrefixes.Count > 0)
        {
            return;
        }

        foreach (string word in validWords)
        {
            if (word.Length < minimumWordLength)
            {
                continue;
            }

            for (int i = 1; i < word.Length; i++)
            {
                string prefix = word[..i];
                viablePrefixes.Add(prefix);
            }
        }
    }

    private record struct CheckPoint(CharacterNode Node, CharacterNode PreviousNode, string CurrentWord);

    public IEnumerable<List<CharacterNode>> EnumerateAllPaths(
        CharacterNode startingNode,
        AdjacencyList<CharacterNode> adjacencyList)
    {
        if (viablePrefixes.Count == 0)
        {
            Setup();
        }

        List<CharacterNode> emptyList = new();
        List<CharacterNode> currentPath = new();
        Stack<CheckPoint> stack = new();
        stack.Push(new CheckPoint(startingNode, startingNode, ""));

        while (stack.Count > 0)
        {
            // revert the current path to the top most checkpoint
            stack.Pop().Deconstruct(
                out CharacterNode currentNode,
                out CharacterNode previousNode,
                out string currentWord);

            while (currentPath.Count > 0 && !currentPath.Last().Equals(previousNode))
            {
                currentPath.RemoveAt(currentPath.Count - 1);
            }

            currentPath.Add(currentNode);
            currentWord += currentNode.Character;

            if (currentWord.Length >= minimumWordLength && validWords.Contains(currentWord))
            {
                yield return new List<CharacterNode>(currentPath);
            }

            if (!viablePrefixes.Contains(currentWord))
            {
                continue;
            }

            List<CharacterNode> adjacentNodes = adjacencyList.GetValueOrDefault(currentNode, emptyList);
            foreach (CharacterNode adjacentNode in adjacentNodes)
            {
                if (!currentPath.Contains(adjacentNode))
                {
                    stack.Push(new CheckPoint(adjacentNode, currentNode, currentWord));
                }
            }

        }
    }

    /// <summary>
    ///     Finds all paths between all pairs of nodes in the adjacency list.
    /// </summary>
    public IEnumerable<List<CharacterNode>> EnumerateAllPaths(AdjacencyList<CharacterNode> adjacencyList)
    {
        foreach (CharacterNode node in adjacencyList.GetAllNodes())
        {
            foreach (List<CharacterNode> path in EnumerateAllPaths(node, adjacencyList))
            {
                yield return path;
            }
        }
    }
}
