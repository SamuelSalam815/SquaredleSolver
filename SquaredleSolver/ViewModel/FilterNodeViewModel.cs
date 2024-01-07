using GraphWalking.Graphs;
using SquaredleSolver.Command;
using SquaredleSolverModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SquaredleSolver.ViewModel;

/// <summary>
///     Represents whether an individual node is included in the search for answers.
/// </summary>
internal class FilterNodeViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly FilterModel filter;

    public CharacterNode CharacterNode { get; }
    public ICommand ToggleInclusionCommand { get; }
    public char Character => CharacterNode.Character;
    public int Row => CharacterNode.Row;
    public int Column => CharacterNode.Column;

    public bool IsIncluded => !filter.ExcludedNodes.Contains(CharacterNode);

    public FilterNodeViewModel(
        CharacterNode node,
        FilterModel filter)
    {
        CharacterNode = node;
        this.filter = filter;
        ToggleInclusionCommand = new ToggleNodeInclusionInFilter(this);
    }

    public void ToggleInclusion()
    {
        if (IsIncluded)
        {
            filter.ExcludedNodes.Add(CharacterNode);
        }
        else
        {
            filter.ExcludedNodes.Remove(CharacterNode);
        }

        OnPropertyChanged(nameof(IsIncluded));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
