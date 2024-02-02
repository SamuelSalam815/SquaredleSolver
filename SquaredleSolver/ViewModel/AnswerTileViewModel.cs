using SquaredleSolverModel;
using System.Collections.Generic;

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

    public static Comparer<AnswerTileViewModel> GetDiscoveryIndexComparer()
    {
        return Comparer<AnswerTileViewModel>.Create(CompareDiscoveryIndex);
    }

    private static int CompareDiscoveryIndex(AnswerTileViewModel a, AnswerTileViewModel b)
    {
        return a.Answer.DiscoveredIndex.CompareTo(b.Answer.DiscoveredIndex);
    }
}
