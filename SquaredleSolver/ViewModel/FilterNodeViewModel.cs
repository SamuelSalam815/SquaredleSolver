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

    public CharacterNode CharacterNode { get; }
    public ICommand ToggleNodeInclusion { get; }
    public char Character => CharacterNode.Character;
    public int Row => CharacterNode.Row;
    public int Column => CharacterNode.Column;

    private bool _isIncluded = true;
    public bool IsIncluded
    {
        get => _isIncluded;
        set
        {
            _isIncluded = value;
            OnPropertyChanged(nameof(IsIncluded));
        }
    }

    public FilterNodeViewModel(
        CharacterNode node,
        FilterModel filter)
    {
        CharacterNode = node;
        ToggleNodeInclusion = new ToggleNodeInclusionInFilter(this, filter);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
