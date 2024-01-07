using SquaredleSolverModel.SolverStates;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolverModel;

/// <summary>
///     Represents an object used to solve a puzzle defined by a <see cref="PuzzleModel"/>
/// </summary>
public class SolverModel
{
    private readonly SolverContext context;
    public ISolverState CurrentState => context.CurrentState;
    public ObservableCollection<AnswerModel> AnswersFound => context.AnswersFound;

    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public TimeSpan TimeSpentSolving => context.Stopwatch.Elapsed;

    public SolverModel(PuzzleModel puzzleModel)
    {
        context = new SolverContext(puzzleModel);
        context.StateChanged += (sender, e) => StateChanged?.Invoke(this, e);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        CurrentState.OnPuzzleChanged(context);
    }

    public Task StartSolvingPuzzle()
    {
        CurrentState.StartSolution(context);
        return context.SolverTask;
    }

    public void StopSolvingPuzzle()
    {
        CurrentState.StopSolution(context);
    }
}
