using GraphWalking.Graphs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolverModel;

public class FilterModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public readonly ObservableCollection<CharacterNode> ExcludedNodes;
    public readonly ObservableCollection<string> AttemptedWords;

    public FilterModel(PuzzleModel puzzleModel)
    {
        ExcludedNodes = new ObservableCollection<CharacterNode>();
        AttemptedWords = new ObservableCollection<string>();

        puzzleModel.PropertyChanged += OnPuzzleChanged;
        ExcludedNodes.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(ExcludedNodes));
        AttemptedWords.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(AttemptedWords));
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        ExcludedNodes.Clear();
        AttemptedWords.Clear();
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool IsAnswerExcluded(AnswerModel answer)
    {
        if (AttemptedWords.Contains(answer.Word))
        {
            return true;
        }

        if (answer.CharacterNodes.Any(ExcludedNodes.Contains))
        {
            return true;
        }

        return false;
    }
}
