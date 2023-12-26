using GraphWalking.Graphs;
using System.Collections.Generic;
using System.Text;

namespace WpfSquaredleSolver.Model;
public class AnswerModel
{
    public List<CharacterNode> CharacterNodes { get; }
    public string Word { get; }

    public AnswerModel(List<CharacterNode> path)
    {
        CharacterNodes = path;
        StringBuilder stringBuilder = new();
        foreach (CharacterNode node in CharacterNodes)
        {
            stringBuilder.Append(node.Character);
        }

        Word = stringBuilder.ToString();
    }
}
