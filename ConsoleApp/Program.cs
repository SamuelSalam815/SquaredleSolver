// see https://github.com/dwyl/english-words for text file containing English words

using GraphWalking;
using GraphWalking.Graphs;
using System.Text;

static HashSet<string> LoadWords()
{
    HashSet<string> words = new();
    using StreamReader reader = new("words_alpha.txt");
    string? line = reader.ReadLine();
    while (line is not null)
    {
        words.Add(line.ToLower());
        line = reader.ReadLine();
    }

    return words;
}

Console.WriteLine("Loading words...");
HashSet<string> validWords = LoadWords();
Console.WriteLine("Building graph...");

AdjacencyList<CharacterNode> graph = CharacterGraphBuilder.FromLetterGrid("""
    pnoc
    rahe
    gngt
    iihu
    """);
PathGenerator<CharacterNode> pathFinder = new(graph);
Console.WriteLine("Generating words...");
StringBuilder stringBuilder = new();
HashSet<string> wordsToAttempt = new();
foreach (List<CharacterNode> path in pathFinder.Generate())
{
    if (path.Count <= 3)
    {
        continue;
    }

    stringBuilder.Clear();
    foreach (CharacterNode node in path)
    {
        stringBuilder.Append(node.Character);
    }

    string word = stringBuilder.ToString();
    if (validWords.Contains(word))
    {
        wordsToAttempt.Add(stringBuilder.ToString());
    }
}