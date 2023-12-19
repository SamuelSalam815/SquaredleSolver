using System.ComponentModel;
using System.IO;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;

class SquaredleViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool isPuzzleBeingSolved;
    private readonly SquaredlePuzzle squaredlePuzzle;

    public SquaredleViewModel()
    {
        squaredlePuzzle = new();
    }

    public bool IsPuzzleBeingSolved
    {
        get { return isPuzzleBeingSolved; }
        set
        {
            isPuzzleBeingSolved = value;
            OnPropertyChanged(nameof(IsPuzzleBeingSolved));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public BindingList<string> WordsFoundInPuzzle { get; } = new();

    private void TogglePuzzleSolverOnOff(object? parameter)
    {
    }

    private bool CanTogglePuzzleSolverOnOff(object? parameter)
    {
        return true;
    }

    public void SetPuzzleText()
    {
        if (IsPuzzleBeingSolved)
        {
            TogglePuzzleSolverOnOff(null);
        }
    }

    public void LoadValidWords(string path)
    {
        squaredlePuzzle.ValidWords.Clear();
        using StreamReader reader = new(path);
        string? line = reader.ReadLine();
        while (line is not null)
        {
            squaredlePuzzle.ValidWords.Add(line.ToUpper());
            line = reader.ReadLine();
        }
    }
}
