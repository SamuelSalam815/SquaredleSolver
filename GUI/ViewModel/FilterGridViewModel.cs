using SquaredleSolver;
using System.ComponentModel;

namespace GUI.ViewModel;
internal class FilterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public FilterModel NodeFilterModel { get; }

    public PuzzleModel PuzzleModel { get; }

    public SolverModel SolverModel { get; }

    public FilterGridViewModel(
        FilterModel nodeFilterModel,
        PuzzleModel puzzleModel,
        SolverModel solverModel)
    {
        NodeFilterModel = nodeFilterModel;
        PuzzleModel = puzzleModel;
        SolverModel = solverModel;

        NodeFilterModel.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(NodeFilterModel));
        PuzzleModel.PropertyChanged += (sender, e) => OnPropertyChanged(nameof(PuzzleModel));
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
