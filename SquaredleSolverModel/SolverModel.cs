using SquaredleSolverModel.SolverStates;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SquaredleSolverModel;

/// <summary>
///     Represents an object used to solve a puzzle defined by a <see cref="PuzzleModel"/>
/// </summary>
public class SolverModel
{
    private SolverContext context;
    public ISolverState CurrentState => context.CurrentState;
    public ObservableCollection<AnswerModel> AnswersFound => context.AnswersFound;

    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public TimeSpan TimeSpentSolving => context.Stopwatch.Elapsed;

    public SolverModel(PuzzleModel puzzleModel, FilterModel filterModel)
    {
        context = new SolverContext(puzzleModel, filterModel);
        context.StateChanged += (sender, e) => StateChanged?.Invoke(this, e);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
        filterModel.PropertyChanged += OnFilterChanged;
    }

    private void OnFilterChanged(object? sender, EventArgs e)
    {
        CurrentState.OnNodeFilterChanged(context);
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
