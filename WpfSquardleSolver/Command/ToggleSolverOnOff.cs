using System;
using System.Windows.Input;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.Command;

class ToggleSolverOnOff : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly SolverModel solver;

    public ToggleSolverOnOff(SolverModel solver)
    {
        this.solver = solver;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (solver.IsSolverRunning)
        {
            solver.StopSolvingPuzzle();
        }
        else
        {
            solver.StartSolvingPuzzle();
        }
    }
}
