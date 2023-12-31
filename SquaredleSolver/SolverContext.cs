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
    public readonly NodeFilterModel FilterModel;
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
;
            StateChanged?.Invoke(
                this,
                new SolverStateChangedEventArgs()
                {
                    CurrentState = CurrentState,
                    PreviousState = previousState
                });
        }
    }

    public SolverContext(PuzzleModel puzzleModel, NodeFilterModel filterModel)
    {
        PuzzleModel = puzzleModel;
        FilterModel = filterModel;
        AnswersFound = new ObservableCollection<AnswerModel>();
        Stopwatch = new Stopwatch();
        CancellationTokenSource = new CancellationTokenSource();
        SolverTask = Task.CompletedTask;
        PathGenerator = new FailFastPathGenerator(
            puzzleModel.ValidWords,
            puzzleModel.MinimumWordLength,
            filterModel.ExcludedNodes);
        backingFieldCurrentState = SolverStopped.Instance;

        PuzzleModel.PropertyChanged += OnPuzzleChanged;
        FilterModel.PropertyChanged += OnNodeFilterChanged;
    }

    private void OnNodeFilterChanged(object? sender, EventArgs e)
    {
        UpdatePathGenerator();
    }

    private void OnPuzzleChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PuzzleModel.ValidWords))
        {
            UpdatePathGenerator();
        }
    }

    private void UpdatePathGenerator()
    {
        PathGenerator = new FailFastPathGenerator(
            PuzzleModel.ValidWords,
            PuzzleModel.MinimumWordLength,
            FilterModel.ExcludedNodes);
    }
}
