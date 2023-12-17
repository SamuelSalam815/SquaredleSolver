// see https://github.com/dwyl/english-words for text file containing English words

using GraphWalking;
using GraphWalking.Graphs;
using System.Diagnostics;
using System.Text;

internal class Program
{
    static List<string> GenerateWordsStartingWith(
        CharacterNode firstCharacter,
        AdjacencyList<CharacterNode> adjacencyList,
        HashSet<string> validWords)
    {
        List<string> generatedWords = new();
        StringBuilder stringBuilder = new();
        IEnumerable<List<CharacterNode>> allPaths =
            PathGenerator<CharacterNode>
            .EnumerateAllPaths(firstCharacter, adjacencyList);
        foreach (List<CharacterNode> path in allPaths)
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
                generatedWords.Add(word);
            }
        }

        return generatedWords;
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Loading words...");
        HashSet<string> validWords = new();
        using (StreamReader reader = new("words_alpha.txt"))
        {
            string? line = reader.ReadLine();
            while (line is not null)
            {
                validWords.Add(line.ToLower());
                line = reader.ReadLine();
            }
        }

        Console.WriteLine("Building graph...");
        AdjacencyList<CharacterNode> graph = CharacterGraphBuilder.FromLetterGrid("""
        pnoc
        rahe
        gngt
        iihu
        """);
        Console.WriteLine("Generating words...");
        HashSet<string> wordsGeneratedInParallel = new();
        Stopwatch stopwatch = Stopwatch.StartNew();
        Parallel.ForEach(graph.GetAllNodes(), startingNode =>
        {
            List<string> generatedWords = GenerateWordsStartingWith(startingNode, graph, validWords);

            lock (wordsGeneratedInParallel)
            {
                foreach (string word in generatedWords)
                {
                    wordsGeneratedInParallel.Add(word);
                }
            }
        });

        stopwatch.Stop();
        TimeSpan timeInParallel = stopwatch.Elapsed;
        Console.WriteLine("Time taken to generate words: " + timeInParallel);

    }
}

