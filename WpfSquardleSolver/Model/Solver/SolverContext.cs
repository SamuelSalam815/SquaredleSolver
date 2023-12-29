using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSquaredleSolver.Model;

/// <summary>
///     Contains the mutable state for the puzzle solver.
/// </summary>
class SolverContext
{
    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public readonly PuzzleModel PuzzleModel;
    public readonly BindingList<AnswerModel> AnswersFoundInPuzzle;
    public CancellationTokenSource CancellationTokenSource;
    public Task SolverTask;
    public DateTime StartTime;
    public DateTime StopTime;

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
        AnswersFoundInPuzzle = new BindingList<AnswerModel>();
        CancellationTokenSource = new CancellationTokenSource();
        SolverTask = Task.CompletedTask;
        backingFieldCurrentState = SolverStopped.Instance;
        StateChanged += OnStateChanged;
    }

    private void OnStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        if (e.PreviousState is SolverRunning)
        {
            StopTime = DateTime.Now;
        }

        if (e.CurrentState is SolverRunning)
        {
            StartTime = DateTime.Now;
        }
    }
}
