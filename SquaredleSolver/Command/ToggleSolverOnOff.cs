using SquaredleSolverModel.Solver;
using System;
using System.Windows.Input;

namespace SquaredleSolver.Command;

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
        return solverModel.State is not SolverState.Completed;
    }

    public void Execute(object? parameter)
    {
        if (solverModel.State is SolverState.Running)
        {
            solverModel.StopSolvingPuzzle();
        }
        else
        {
            solverModel.StartSolvingPuzzle();
        }
    }
}
