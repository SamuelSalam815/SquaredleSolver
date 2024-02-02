using GraphWalking.Graphs;
using System.Text;

namespace SquaredleSolverModel;

/// <summary>
///     Represents a possible answer to a given squaredle puzzle.
/// </summary>
public class AnswerModel
{
    public List<CharacterNode> CharacterNodes { get; }

    /// <summary>
    ///     When this is 0, this is the first answer that was found.
    ///     Increases with the number of answers found.
    /// </summary>
    public int DiscoveredIndex { get; }

    public string Word { get; }

    public AnswerModel(List<CharacterNode> path, int discoveredIndex)
    {
        CharacterNodes = path;
        DiscoveredIndex = discoveredIndex;
        StringBuilder stringBuilder = new();
        foreach (CharacterNode node in CharacterNodes)
        {
            stringBuilder.Append(node.Character);
        }

        Word = stringBuilder.ToString();
    }
}
