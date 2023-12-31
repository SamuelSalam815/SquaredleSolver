using GraphWalking.Graphs;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolver;

public class NodeFilterModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly List<CharacterNode> excludedNodes;
    public readonly ReadOnlyCollection<CharacterNode> ExcludedNodes;

    public NodeFilterModel(PuzzleModel puzzleModel)
    {
        excludedNodes = new List<CharacterNode>();
        ExcludedNodes = new ReadOnlyCollection<CharacterNode>(excludedNodes);

        puzzleModel.PropertyChanged += OnPuzzleChanged;
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        excludedNodes.Clear();
        OnPropertyChanged(nameof(ExcludedNodes));
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
