using System.Collections.Generic;

namespace WpfSquardleSolver.Model;

class PuzzleModel
{
    public HashSet<string> ValidWords { get; set; } = new();
    public string PuzzleAsText { get; set; } = "";
}
