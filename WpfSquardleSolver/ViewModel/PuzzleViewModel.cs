using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;

class PuzzleViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly PuzzleModel puzzleModel;

    public BindingList<string> WordsFoundInPuzzle { get; } = new();

    public string PuzzleAsText
    {
        get { return puzzleModel.PuzzleAsText; }
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    public HashSet<string> ValidWords
    {
        get { return puzzleModel.ValidWords; }
    }

    public PuzzleViewModel()
    {
        puzzleModel = new PuzzleModel();
    }

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

        puzzleModel.ValidWords = newValidWords;
        OnPropertyChanged(nameof(ValidWords));
    }
}
