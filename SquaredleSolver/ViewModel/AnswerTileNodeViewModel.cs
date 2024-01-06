using GraphWalking.Graphs;
using SquaredleSolverModel;

namespace SquaredleSolver.ViewModel;
internal class AnswerTileNodeViewModel
{
    private readonly CharacterNode characterNode;
    public bool IsOnHighlightedPath { get; }
    public bool IsExcluded { get; }
    public int Row => characterNode.Row;
    public int Column => characterNode.Column;

    public char Character => characterNode.Character;

    public AnswerTileNodeViewModel(CharacterNode node, AnswerModel answer, FilterModel filter)
    {
        characterNode = node;
        IsOnHighlightedPath = answer.CharacterNodes.Contains(node);
        IsExcluded = filter.ExcludedNodes.Contains(node);
    }
}
