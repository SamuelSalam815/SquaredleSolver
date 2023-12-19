using System;
using System.Windows.Input;

namespace WpfSquardleSolver.ViewModel;

public class TogglePuzzleSolverOnOff : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly PuzzleSolver puzzleSolver;

    public TogglePuzzleSolverOnOff(PuzzleSolver puzzleSolver)
    {
        this.puzzleSolver = puzzleSolver;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if(puzzleSolver.IsSolverRunning)
        {
            puzzleSolver.StopSolvingPuzzle();
        } else
        {
            puzzleSolver.StartSolvingPuzzle();
        }
    }
}
