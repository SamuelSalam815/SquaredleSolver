using System;
using System.Windows.Input;
using WpfSquardleSolver.Model;
using WpfSquardleSolver.ViewModel;

namespace WpfSquardleSolver.Command;

class ToggleSolverOnOff : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly SolverModel solverModel;
    private readonly MainWindowViewModel solverViewModel;

    public ToggleSolverOnOff(
        SolverModel solverModel,
        MainWindowViewModel solverViewModel)
    {
        this.solverModel = solverModel;
        this.solverViewModel = solverViewModel;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (solverViewModel.IsSolverRunning)
        {
            solverModel.StopSolvingPuzzle();
        }
        else
        {
            solverModel.StartSolvingPuzzle();
        }
    }
}
