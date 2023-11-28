namespace GraphWalking.Graphs;

/// <summary>
///     Represents a graph of <see cref="CharacterNode"/>s
/// </summary>
public class CharacterGraph
{
    private readonly AdjacencyList<CharacterNode> graph;

    private CharacterGraph()
    {
        graph = new AdjacencyList<CharacterNode>();
    }

    public static CharacterGraph FromLetterGrid(string letterGrid)
    {
        CharacterGraph result = new();
        string[] letterRows = letterGrid.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        int numRows = letterRows.Length;
        int numColumns = letterRows[0].Length;

        if (letterRows.Any(line => line.Length != numColumns))
        {
            throw new ArgumentException("All lines in the input string do not have equal length");
        }

        // create a node for each letter in the letter grid
        // use integer node ids so duplicate letters can be distinguished
        foreach (var character in letterRows.SelectMany(line => line.ToCharArray()))
        {
            int index = result.graph.Count;
            CharacterNode node = new(index, character);
            result.graph[node] = new List<CharacterNode>();
        }

        bool IsInBounds((int row, int column) position)
        {
            return
                0 <= position.row && position.row < numRows
                    &&
                0 <= position.column && position.column < numColumns;
        }

        CharacterNode ToNode((int row, int column) position)
        {
            int id = position.row * numColumns + position.column;
            char character = letterRows[position.row][position.column];
            return new CharacterNode(id, character);
        }

        // add all the node adjacencies according to the grid
        for (int row = 0; row < numRows; row++)
        {
            for (int column = 0; column < numColumns; column++)
            {
                IEnumerable<CharacterNode> adjacentNodes = GetAdjacentPositions((row, column))
                    .Where(IsInBounds)
                    .Select(ToNode);

                CharacterNode node = ToNode((row, column));
                result.graph[node].AddRange(adjacentNodes);
            }
        }

        return result;
    }

    private static IEnumerable<(int, int)> GetAdjacentPositions((int row, int column) position)
    {
        int row = position.row;
        int column = position.column;
        return new List<(int, int)> {
            (row, column + 1),
            (row, column - 1),
            (row - 1, column),
            (row - 1, column + 1),
            (row - 1, column - 1),
            (row + 1, column),
            (row + 1, column + 1),
            (row + 1, column - 1),
        };
    }

    // Currently only used for testing the construction of the character graph
    public List<char> GetAdjacentCharacters(char character)
    {
        var matchingNodes = graph.GetAllNodes().Where(node => node.Character == character);

        var adjacentNodes = matchingNodes.SelectMany(node => graph[node]);

        List<char> adjacentCharacters = adjacentNodes.Select(node => node.Character).ToList();

        return new HashSet<char>(adjacentCharacters).ToList();
    }
}