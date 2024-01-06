using GraphWalking.Graphs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolverModel;

public class FilterModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly List<CharacterNode> excludedNodes;
    public readonly ReadOnlyCollection<CharacterNode> ExcludedNodes;
    public readonly ObservableCollection<string> AnswersAlreadyAttempted;

    public FilterModel(PuzzleModel puzzleModel)
    {
        excludedNodes = new List<CharacterNode>();
        ExcludedNodes = new ReadOnlyCollection<CharacterNode>(excludedNodes);
        AnswersAlreadyAttempted = new ObservableCollection<string>();

        puzzleModel.PropertyChanged += OnPuzzleChanged;
        AnswersAlreadyAttempted.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(AnswersAlreadyAttempted));
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        excludedNodes.Clear();
        OnPropertyChanged(nameof(ExcludedNodes));

        AnswersAlreadyAttempted.Clear();
    }

    public void IncludeNode(CharacterNode node)
    {
        excludedNodes.Remove(node);
        OnPropertyChanged(nameof(ExcludedNodes));
    }

    public void ExcludeNode(CharacterNode node)
    {
        if (!excludedNodes.Contains(node))
        {
            excludedNodes.Add(node);
            OnPropertyChanged(nameof(ExcludedNodes));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
