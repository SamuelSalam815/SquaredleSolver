namespace GraphWalking.Graphs;

public class CharacterGraphBuilder
{
    public static AdjacencyList<CharacterNode> FromLetterGrid(string letterGrid)
    {
        AdjacencyList<CharacterNode> adjacencyList = new();
        string[] lines = letterGrid.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        if (lines.Any(line => line.Length != lines[0].Length))
        {
            throw new ArgumentException("All lines in the input string do not have equal length");
        }

        // initialize node adjacencies
        IEnumerable<CharacterNode> allNodes = lines.SelectMany(CreateNodesFromLine);
        foreach (var node in allNodes)
        {
            adjacencyList[node] = new List<CharacterNode>();
        }

        // discover all node adjacencies and add them to the adjacency list
        Dictionary<(int, int), CharacterNode> nodesByPosition = allNodes.ToDictionary(node => (node.Row, node.Column), node => node);
        foreach (CharacterNode node in allNodes)
        {
            IEnumerable<CharacterNode> adjacentNodes = GetAdjacentPositions((node.Row, node.Column))
                .Where(nodesByPosition.ContainsKey)
                .Select(position => nodesByPosition[position]);

            adjacencyList[node].AddRange(adjacentNodes);
        }

        return adjacencyList;
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
}
