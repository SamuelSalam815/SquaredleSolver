using GraphWalking.Graphs;
using SquaredleSolver;

namespace GUI.ViewModel;
internal class CharacterNodeViewModel
{
    public bool IsOnHighlightedPath { get; }
    public bool IsExcluded { get; }

    public CharacterNodeViewModel(CharacterNode node, AnswerModel answer, FilterModel filter)
    {
        IsOnHighlightedPath = answer.CharacterNodes.Contains(node);
        IsExcluded = filter.ExcludedNodes.Contains(node);
    }
}
