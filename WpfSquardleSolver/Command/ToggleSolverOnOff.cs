using System;
using System.Windows.Input;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.Command;

/// <summary>
///     Defines a command for toggling the puzzle solver on and off.
/// </summary>
class ToggleSolverOnOff : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly SolverModel solverModel;

    public ToggleSolverOnOff(
        SolverModel solverModel)
    {
        this.solverModel = solverModel;
    }

    public bool CanExecute(object? parameter)
    {
        return solverModel.CurrentState is not SolverCompleted;
    }

    public void Execute(object? parameter)
    {
        if (solverModel.CurrentState is SolverRunning)
        {
            solverModel.StopSolvingPuzzle();
        }
        else
        {
            solverModel.StartSolvingPuzzle();
        }
    }
}
