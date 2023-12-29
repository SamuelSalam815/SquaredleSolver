using GraphWalking.Graphs;
using System.ComponentModel;

namespace SquaredleSolver;

/// <summary>
///     Represents the current squaredle puzzle to be solved
/// </summary>
public class PuzzleModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string puzzleAsTextBackingField = string.Empty;
    public string PuzzleAsText
    {
        get { return puzzleAsTextBackingField; }
        set
        {
            puzzleAsTextBackingField = value;
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

    private uint numberOfRowsBackingField;
    public uint NumberOfRows
    {
        get { return numberOfRowsBackingField; }
        private set
        {
            numberOfRowsBackingField = value;
            OnPropertyChanged(nameof(NumberOfRows));
        }
    }

    private uint numberOfColumnsBackingField;
    public uint NumberOfColumns
    {
        get { return numberOfColumnsBackingField; }
        private set
        {
            numberOfColumnsBackingField = value;
            OnPropertyChanged(nameof(NumberOfColumns));
        }
    }

    private AdjacencyList<CharacterNode> puzzleAsAdjacencyListBackingField = new();
    public AdjacencyList<CharacterNode> PuzzleAsAdjacencyList
    {
        get { return puzzleAsAdjacencyListBackingField; }
        private set
        {
            puzzleAsAdjacencyListBackingField = value;
            OnPropertyChanged(nameof(PuzzleAsAdjacencyList));
        }
    }

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
            newValidWords.Add(line.ToUpper());
            line = reader.ReadLine();
        }

        ValidWords = newValidWords;
        OnPropertyChanged(nameof(ValidWords));
    }
}
