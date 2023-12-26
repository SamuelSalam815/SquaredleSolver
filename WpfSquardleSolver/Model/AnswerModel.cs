using GraphWalking.Graphs;
using System.Collections.Generic;
using System.Text;

namespace WpfSquardleSolver.Model;
internal class AnswerModel
{
    public List<CharacterNode> Path { get; }
    public string Word { get; }

    public AnswerModel(List<CharacterNode> path)
    {
        Path = path;
        StringBuilder stringBuilder = new();
        foreach (CharacterNode node in Path)
        {
            stringBuilder.Append(node.Character);
        }

        Word = stringBuilder.ToString();
    }
}
