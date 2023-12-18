using GraphWalking.Graphs;
using System.Collections.Generic;

namespace WpfSquardleSolver;

public record struct PuzzleAnswer(List<CharacterNode> Path, string Word);
