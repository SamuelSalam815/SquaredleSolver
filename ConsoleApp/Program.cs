// see https://github.com/dwyl/english-words for text file containing English words

using GraphWalking;
using GraphWalking.Graphs;

HashSet<string> LoadWords()
{
    HashSet<string> words = new();
    using var reader = new StreamReader("words_alpha.txt");
    string? line = reader.ReadLine();
    while (line is not null)
    {
        words.Add(line.ToLower());
        line = reader.ReadLine();
    }

    return words;
}

Console.WriteLine("Loading words...");
HashSet<string> words = LoadWords();
Console.WriteLine("Building graph...");
AdjacencyList<CharacterNode> graph = CharacterGraphBuilder.FromLetterGrid("""
    pnoc
    rahe
    gngt
    iihu
    """);
PathGenerator<CharacterNode> pathFinder = new(graph);

Console.WriteLine("Generating words...");
HashSet<string> wordsToAttempt = pathFinder
    .Generate()
    .Where(path => path.Count > 3)
    .Select(path => string.Join("", path.Select(node => node.Character)))
    .Where(words.Contains)
    .ToHashSet();

foreach (string word in wordsToAttempt)
{
    Console.WriteLine(word);
}