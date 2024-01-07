using GraphWalking.Graphs;
using SquaredleSolverModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SquaredleSolver.ViewModel;
internal class AnswerTileCharacterNodeViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly CharacterNode characterNode;
    private readonly FilterModel filter;

    public bool IsOnHighlightedPath { get; }
    public bool IsExcluded => filter.ExcludedNodes.Contains(characterNode);
    public int Row => characterNode.Row;
    public int Column => characterNode.Column;

    public char Character => characterNode.Character;

    public AnswerTileCharacterNodeViewModel(CharacterNode node, AnswerModel answer, FilterModel filter)
    {
        characterNode = node;
        this.filter = filter;
        IsOnHighlightedPath = answer.CharacterNodes.Contains(node);

        filter.ExcludedNodes.CollectionChanged += OnExcludedNodesChanged;
    }

    private void OnExcludedNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems?.Contains(characterNode) == true)
                {
                    OnPropertyChanged(nameof(IsExcluded));
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems?.Contains(characterNode) == true)
                {
                    OnPropertyChanged(nameof(IsExcluded));
                }
                break;
            default:
                OnPropertyChanged(nameof(IsExcluded));
                break;
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
