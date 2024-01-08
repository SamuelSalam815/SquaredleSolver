using SquaredleSolverModel;
using System.ComponentModel;

namespace SquaredleSolver.ViewModel;
internal class FilterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public FilterViewModel Filter { get; }

    public PuzzleModel Puzzle { get; }

    public FilterGridViewModel(FilterViewModel filter, PuzzleModel puzzle)
    {
        Filter = filter;
        Puzzle = puzzle;

        Filter.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Filter));
        Puzzle.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(Puzzle));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
