// see https://github.com/dwyl/english-words for text file containing English words

using GraphWalking;
using GraphWalking.Graphs;
using SquaredleSolverModel;
using System.Diagnostics;

internal class Program
{

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Loading puzzle...");
        PuzzleModel puzzleModel = new();
        puzzleModel.LoadValidWords("words_alpha.txt");
        puzzleModel.PuzzleAsText = """
            PNOC
            RAHE
            GNGT
            IIHU
            """;

        // Fail-Fast
        Console.WriteLine("Running fail fast generator...");
        FailFastPathGenerator pathGenerator = new(puzzleModel.ValidWords, 4);
        HashSet<string> failFastGeneratedWords = new();
        Stopwatch stopWatch = new();
        stopWatch.Start();
        foreach (var path in pathGenerator.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList))
        {
            failFastGeneratedWords.Add(string.Concat(path.Select(x => x.Character)));
        }
        stopWatch.Stop();
        Console.WriteLine("Time elapsed: " + stopWatch.Elapsed);

        // Brute force
        Console.WriteLine("Running brute force generator...");
        HashSet<string> bruteForcedWords = new();
        stopWatch.Restart();
        foreach (List<CharacterNode> path in BruteForcePathGenerator<CharacterNode>.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList))
        {
            string word = string.Concat(path.Select(x => x.Character));
            if (word.Length > 3 && puzzleModel.ValidWords.Contains(word))
            {
                bruteForcedWords.Add(word);
            }
        }
        stopWatch.Stop();
        Console.WriteLine("Time elapsed: " + stopWatch.Elapsed);

        // Cross validation
        Console.WriteLine("Cross-Validating results...");
        int inconsistencyCount = 0;
        Console.WriteLine("Words missing from fail fast: ");
        foreach (string word in bruteForcedWords)
        {
            if (!failFastGeneratedWords.Contains(word))
            {
                inconsistencyCount++;
                Console.WriteLine(word);
            }
        }
        Console.WriteLine("Words missing from brute force answers: ");
        foreach (string word in failFastGeneratedWords)
        {
            if (!bruteForcedWords.Contains(word))
            {
                inconsistencyCount++;
                Console.WriteLine(word);
            }
        }
        Console.WriteLine("Cross-Validation inconsistencies: " + inconsistencyCount);
    }
}

