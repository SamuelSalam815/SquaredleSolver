using System;
using System.Windows.Input;
using WpfSquaredleSolver.Model;
using WpfSquaredleSolver.ViewModel;

namespace WpfSquaredleSolver.Command;

/// <summary>
///     Defines a command for toggling the puzzle solver on and off.
/// </summary>
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
