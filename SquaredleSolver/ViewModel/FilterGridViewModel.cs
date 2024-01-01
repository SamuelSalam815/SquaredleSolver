using SquaredleSolverModel;
using System.ComponentModel;

namespace SquaredleSolver.ViewModel;
internal class FilterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public FilterModel NodeFilterModel { get; }

    public PuzzleModel PuzzleModel { get; }

    public FilterGridViewModel(
        FilterModel nodeFilterModel,
        PuzzleModel puzzleModel)
    {
        NodeFilterModel = nodeFilterModel;
        PuzzleModel = puzzleModel;

        NodeFilterModel.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(NodeFilterModel));
        PuzzleModel.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(PuzzleModel));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
