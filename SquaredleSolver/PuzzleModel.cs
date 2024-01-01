using GraphWalking.Graphs;
using System.ComponentModel;

namespace SquaredleSolver;

/// <summary>
///     Represents the current squaredle puzzle to be solved
/// </summary>
public class PuzzleModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _puzzleAsText = string.Empty;
    public string PuzzleAsText
    {
        get { return _puzzleAsText; }
        set
        {
            _puzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
            PuzzleAsAdjacencyList = CharacterGraphBuilder.FromLetterGrid(PuzzleAsText);

            string[] lines = PuzzleAsText.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            NumberOfRows = (uint)lines.Length;
            int longestLineLength = 0;
            foreach (string line in lines)
            {
                longestLineLength = Math.Max(line.Length, longestLineLength);
            }
            NumberOfColumns = (uint)longestLineLength;
        }
    }

    private uint _numberOfRows;
    public uint NumberOfRows
    {
        get { return _numberOfRows; }
        private set
        {
            _numberOfRows = value;
            OnPropertyChanged(nameof(NumberOfRows));
        }
    }

    private uint _numberOfColumns;
    public uint NumberOfColumns
    {
        get { return _numberOfColumns; }
        private set
        {
            _numberOfColumns = value;
            OnPropertyChanged(nameof(NumberOfColumns));
        }
    }

    private AdjacencyList<CharacterNode> _puzzleAsAdjacencyList = new();
    public AdjacencyList<CharacterNode> PuzzleAsAdjacencyList
    {
        get { return _puzzleAsAdjacencyList; }
        private set
        {
            _puzzleAsAdjacencyList = value;
            OnPropertyChanged(nameof(PuzzleAsAdjacencyList));
            PuzzleAsNodes = _puzzleAsAdjacencyList.GetAllNodes();
        }
    }

    private List<CharacterNode> _puzzleAsNodes = new();
    public List<CharacterNode> PuzzleAsNodes
    {
        get { return _puzzleAsNodes; }
        private set
        {
            _puzzleAsNodes = value;
            OnPropertyChanged(nameof(PuzzleAsNodes));
        }
    }

    public static int MinimumWordLength => 4;

    public HashSet<string> ValidWords { get; private set; } = new();

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void LoadValidWords(string path)
    {
        HashSet<string> newValidWords = new();
        using StreamReader reader = new(path);
        string? line = reader.ReadLine();
        while (line is not null)
        {
            if (line.Length >= MinimumWordLength)
            {
                newValidWords.Add(line.ToUpper());
            }
            line = reader.ReadLine();
        }

        ValidWords = newValidWords;
        OnPropertyChanged(nameof(ValidWords));
    }
}
