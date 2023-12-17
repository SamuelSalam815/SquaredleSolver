namespace GraphWalking.Graphs;

public class CharacterGraphBuilder
{
    public static AdjacencyList<CharacterNode> FromLetterGrid(string letterGrid)
    {
        AdjacencyList<CharacterNode> adjacencyList = new();
        string[] lines = letterGrid.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            for (int characterIndex = 0; characterIndex < lines[lineIndex].Length; characterIndex++)
            {
                char character = lines[lineIndex][characterIndex];
                if (char.IsWhiteSpace(character))
                {
                    continue;
                }

                CharacterNode node = new(lineIndex, characterIndex, character);
                adjacencyList[node] = new List<CharacterNode>();
            }
        }

        Dictionary<(int, int), CharacterNode> nodesByPosition = adjacencyList
            .Keys
            .ToDictionary(node => (node.Row, node.Column), node => node);
        foreach (CharacterNode node in nodesByPosition.Values)
        {
            foreach ((int Row, int Column) vector in adjacencyMask)
            {
                var nodePosition = (node.Row + vector.Row, node.Column + vector.Column);
                if (nodesByPosition.TryGetValue(nodePosition, out var adjacentNode))
                {
                    adjacencyList[node].Add(adjacentNode);
                }
            }
        }

        return adjacencyList;
    }

    private static readonly List<(int, int)> adjacencyMask = new()
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
}
