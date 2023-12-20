using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace WpfSquardleSolver.Model;

class PuzzleModel : INotifyPropertyChanged
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
        }
    }

    private AdjacencyList<CharacterNode> puzzleAsAdjacencyListBackingField = new();
    public AdjacencyList<CharacterNode> PuzzleAsAdjacencyList
    {
        get { return puzzleAsAdjacencyListBackingField; }
        set
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
