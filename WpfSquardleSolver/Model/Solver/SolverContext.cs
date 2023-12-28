using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSquaredleSolver.Model;

/// <summary>
///     Contains the mutable state for the puzzle solver.
/// </summary>
class SolverContext
{
    public readonly PuzzleModel PuzzleModel;
    public readonly ObservableCollection<AnswerModel> AnswersFoundInPuzzle;
    public CancellationTokenSource CancellationTokenSource;
    public Task SolverTask;

    private ISolverState backingFieldCurrentState;
    public ISolverState CurrentState
    {
        get { return backingFieldCurrentState; }
        set
        {
            ISolverState previousState = backingFieldCurrentState;
            backingFieldCurrentState = value;
            StateChanged?.Invoke(
                this,
                new SolverStateChangedEventArgs()
                {
                    CurrentState = CurrentState,
                    PreviousState = previousState
                });
        }
    }

    public SolverContext(PuzzleModel puzzleModel)
    {
        PuzzleModel = puzzleModel;
        AnswersFoundInPuzzle = new ObservableCollection<AnswerModel>();
        CancellationTokenSource = new CancellationTokenSource();
        SolverTask = Task.CompletedTask;
        backingFieldCurrentState = SolverStopped.Instance;
    }

    public EventHandler<SolverStateChangedEventArgs>? StateChanged;
}
