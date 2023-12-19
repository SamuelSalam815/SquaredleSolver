using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;

public class SquaredleViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool isPuzzleBeingSolved;
    private readonly SquaredlePuzzle squaredlePuzzle;
    private readonly PuzzleSolver puzzleSolver;

    public bool IsPuzzleBeingSolved
    {
        get { return isPuzzleBeingSolved; }
        set
        {
            isPuzzleBeingSolved = value;
            OnPropertyChanged(nameof(IsPuzzleBeingSolved));
        }
    }

    public ICommand TogglePuzzleSolverOnOffCommand { get; }
    public SquaredleViewModel()
    {
        squaredlePuzzle = new SquaredlePuzzle();
        puzzleSolver = new PuzzleSolver(new System.Collections.Generic.HashSet<string>());
        TogglePuzzleSolverOnOffCommand = new TogglePuzzleSolverOnOff(puzzleSolver);
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

    public void SetPuzzleText(string puzzleAsText)
    {
        if (IsPuzzleBeingSolved)
        {
            TogglePuzzleSolverOnOff(null);
        }

        squaredlePuzzle.PuzzleAsText = puzzleAsText;
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
