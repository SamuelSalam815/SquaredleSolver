namespace GraphWalking.Graphs;

/// <summary>
///     Represents a graph of <see cref="CharacterNode"/>s
/// </summary>
public class CharacterGraph
{
    private readonly AdjacencyList<CharacterNode> adjacencyList;

    private CharacterGraph()
    {
        adjacencyList = new AdjacencyList<CharacterNode>();
    }

    public static CharacterGraph FromLetterGrid(string letterGrid)
    {
        CharacterGraph result = new();
        string[] lines = letterGrid.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        if (lines.Any(line => line.Length != lines[0].Length))
        {
            throw new ArgumentException("All lines in the input string do not have equal length");
        }

        // initialize node adjacencies
        IEnumerable<CharacterNode> allNodes = lines.SelectMany(CreateNodesFromLine);
        foreach (var node in allNodes)
        {
            result.adjacencyList[node] = new List<CharacterNode>();
        }

        // discover all node adjacencies and add them to the adjacency list
        Dictionary<(int, int), CharacterNode> nodesByPosition = allNodes.ToDictionary(node => (node.Row, node.Column), node => node);
        foreach (CharacterNode node in allNodes)
        {
            IEnumerable<CharacterNode> adjacentNodes = GetAdjacentPositions((node.Row, node.Column))
                .Where(nodesByPosition.ContainsKey)
                .Select(position => nodesByPosition[position]);

            result.adjacencyList[node].AddRange(adjacentNodes);
        }

        return result;
    }

    private static IEnumerable<CharacterNode> CreateNodesFromLine(string line, int rowIndex)
    {
        return line.Select((character, columnIndex) => new CharacterNode(rowIndex, columnIndex, character));
    }

    private static readonly List<(int, int)> adjacencyMask = new()
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };

    private static IEnumerable<(int, int)> GetAdjacentPositions((int row, int column) position)
    {
        return adjacencyMask.Select(((int row, int column) translation) => (position.row + translation.row, position.column + translation.column));
    }


    // Currently only used for testing the construction of the character graph
    public List<char> GetAdjacentCharacters(char character)
    {
        IEnumerable<CharacterNode> matchingNodes = adjacencyList.GetAllNodes().Where(node => node.Character == character);
        IEnumerable<CharacterNode> adjacentNodes = matchingNodes.SelectMany(node => adjacencyList[node]);
        HashSet<char> adjacentCharacters = adjacentNodes.Select(node => node.Character).ToHashSet();
        return adjacentCharacters.ToList();
    }
}