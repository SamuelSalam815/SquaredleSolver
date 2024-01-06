using SquaredleSolverModel;

namespace SquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties to render a visual representation of a squaredle answer.
/// </summary>
class AnswerTileViewModel
{
    public AnswerModel Answer { get; }
    public PuzzleModel Puzzle { get; }
    public FilterModel Filter { get; }

    public AnswerTileViewModel(
        AnswerModel answerModel,
        PuzzleModel puzzleModel,
        FilterModel filterModel)
    {
        Answer = answerModel;
        Puzzle = puzzleModel;
        Filter = filterModel;
    }
}
