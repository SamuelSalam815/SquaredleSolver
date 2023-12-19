using System.Collections.Generic;

namespace WpfSquardleSolver.Model;

class SquaredlePuzzle
{
    public HashSet<string> ValidWords { get; } = new();
    public string PuzzleAsText { get; set; } = "";
}
