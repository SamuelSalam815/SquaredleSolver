using SquaredleSolver.SolverStates;
using System.Collections.ObjectModel;

namespace SquaredleSolver;

/// <summary>
///     Contains the mutable state for the puzzle solver.
/// </summary>
public class SolverContext
{
    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public readonly PuzzleModel PuzzleModel;
    public readonly ObservableCollection<AnswerModel> AnswersFoundInPuzzle;
    public CancellationTokenSource CancellationTokenSource;
    public Task SolverTask;
    public DateTime StartTime;
    public DateTime StopTime;

    public bool AddAnswersOnOwningThread { init; get; }

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
