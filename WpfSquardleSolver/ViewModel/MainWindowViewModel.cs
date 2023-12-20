using System.ComponentModel;
using System.Windows.Input;
using WpfSquardleSolver.Command;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;
internal class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ToggleSolverOnOff { get; }

    public string PuzzleAsText
    {
        get { return puzzleModel.PuzzleAsText; }
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    private bool isSolverRunningBackingField = false;
    public bool IsSolverRunning
    {
        get { return isSolverRunningBackingField; }
        set
        {
            isSolverRunningBackingField = value;
            OnPropertyChanged(nameof(IsSolverRunning));
        }
    }

    public BindingList<string> ValidWordsFoundInPuzzle
        => solverModel.ValidWordsFoundInPuzzle;

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(SolverModel solverModel)
    {
        this.solverModel = solverModel;
        solverModel.SolverStarted += () => IsSolverRunning = true;
        solverModel.SolverStopped += () => IsSolverRunning = false;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel, this);
    }

    private void OnPropertyChanged(string nameOfProperty)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nameOfProperty));
    }

}
