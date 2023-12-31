using SquaredleSolver;
using System.ComponentModel;

namespace GUI.ViewModel;
internal class NodeFilterGridViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public NodeFilterModel NodeFilterModel { get; }

    public PuzzleModel PuzzleModel { get; }

    public NodeFilterGridViewModel(
        NodeFilterModel nodeFilterModel,
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
