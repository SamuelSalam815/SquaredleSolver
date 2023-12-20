using System.ComponentModel;
using System.Windows.Input;
using WpfSquardleSolver.Command;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;
internal class SolverViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ToggleSolverOnOff { get; }

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

    public SolverViewModel(SolverModel solverModel)
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
