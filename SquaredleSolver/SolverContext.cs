using GraphWalking;
using SquaredleSolver.SolverStates;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SquaredleSolver;

/// <summary>
///     Contains the mutable state for the puzzle solver.
/// </summary>
public class SolverContext
{
    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public readonly PuzzleModel PuzzleModel;
    public readonly ObservableCollection<AnswerModel> AnswersFound;
    public readonly Stopwatch Stopwatch;
    public CancellationTokenSource CancellationTokenSource;
    public Task SolverTask;
    public FailFastPathGenerator PathGenerator;

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
        PuzzleModel.PropertyChanged += OnPuzzleModelChanged;
        AnswersFound = new ObservableCollection<AnswerModel>();
        Stopwatch = new Stopwatch();
        CancellationTokenSource = new CancellationTokenSource();
        SolverTask = Task.CompletedTask;

        backingFieldCurrentState = SolverStopped.Instance;
        StateChanged += OnStateChanged;

        PathGenerator = new FailFastPathGenerator(puzzleModel.ValidWords, puzzleModel.MinimumWordLength);
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PuzzleModel.ValidWords))
        {
            PathGenerator = new FailFastPathGenerator(PuzzleModel.ValidWords, PuzzleModel.MinimumWordLength);
        }
    }

    private void OnStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        if (e.PreviousState is SolverRunning)
        {
            Stopwatch.Stop();
        }

        if (e.CurrentState is SolverRunning)
        {
            Stopwatch.Restart();
        }
    }
}
