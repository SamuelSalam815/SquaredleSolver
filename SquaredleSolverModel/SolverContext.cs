using GraphWalking;
using SquaredleSolverModel.SolverStates;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SquaredleSolverModel;

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
    public FailFastPathGenerator PathGenerator { get; private set; }

    private ISolverState backingFieldCurrentState;
    public ISolverState CurrentState
    {
        get { return backingFieldCurrentState; }
        set
        {
            ISolverState previousState = backingFieldCurrentState;
            backingFieldCurrentState = value;

            if (previousState is SolverRunning)
            {
                Stopwatch.Stop();
            }

            if (CurrentState is SolverRunning)
            {
                Stopwatch.Restart();
            }

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
        AnswersFound = new ObservableCollection<AnswerModel>();
        Stopwatch = new Stopwatch();
        CancellationTokenSource = new CancellationTokenSource();
        SolverTask = Task.CompletedTask;
        PathGenerator = new FailFastPathGenerator(puzzleModel.ValidWords, PuzzleModel.MinimumWordLength);
        backingFieldCurrentState = SolverStopped.Instance;

        PuzzleModel.PropertyChanged += OnPuzzleChanged;
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PuzzleModel.ValidWords))
        {
            PathGenerator = new FailFastPathGenerator(PuzzleModel.ValidWords, PuzzleModel.MinimumWordLength);
        }
    }
}
