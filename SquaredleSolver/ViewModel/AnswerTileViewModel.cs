using SquaredleSolverModel;

namespace SquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties to render a visual representation of a squaredle answer.
/// </summary>
class AnswerTileViewModel
{
    public AnswerModel Answer { get; }
    public PuzzleModel Puzzle { get; }
    public FilterViewModel Filter { get; }

    public AnswerTileViewModel(
        AnswerModel answer,
        PuzzleModel puzzle,
        FilterViewModel filter)
    {
        Answer = answer;
        Puzzle = puzzle;
        Filter = filter;
    }
}
