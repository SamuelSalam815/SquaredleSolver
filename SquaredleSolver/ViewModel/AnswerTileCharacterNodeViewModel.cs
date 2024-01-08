using SquaredleSolverModel;
using System.ComponentModel;

namespace SquaredleSolver.ViewModel;
internal class AnswerTileCharacterNodeViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly FilterNodeViewModel filterNode;

    public bool IsOnHighlightedPath { get; }
    public bool IsExcluded => !filterNode.IsIncluded;
    public int Row => filterNode.Row;
    public int Column => filterNode.Column;

    public char Character => filterNode.Character;

    public AnswerTileCharacterNodeViewModel(
        FilterNodeViewModel filterNode,
        AnswerModel answer)
    {
        this.filterNode = filterNode;
        IsOnHighlightedPath = answer.CharacterNodes.Contains(filterNode.CharacterNode);

        filterNode.PropertyChanged += OnFilterNodeChanged;
    }

    private void OnFilterNodeChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsExcluded));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
