using GraphWalking.Graphs;
using SquaredleSolver.Command;
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
    public ICommand ToggleInclusionCommand { get; }
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

    public FilterNodeViewModel(CharacterNode node)
    {
        CharacterNode = node;
        ToggleInclusionCommand = new ToggleNodeInclusionInFilter(this);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
