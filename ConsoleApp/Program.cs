// see https://github.com/dwyl/english-words for text file containing English words

using GraphWalking;
using SquaredleSolver;
using SquaredleSolver.SolverStates;
using System.Diagnostics;

internal class Program
{

    private static async Task Main(string[] args)
    {
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

        Console.WriteLine("Loading words...");
        PuzzleModel puzzleModel = new();
        puzzleModel.LoadValidWords("words_alpha.txt");
        puzzleModel.PuzzleAsText = """
            PNOC
            RAHE
            GNGT
            IIHU
            """;

        Console.WriteLine("Running fail fast generator...");
        FailFastPathGenerator pathGenerator = new(puzzleModel.ValidWords, 4);
        HashSet<string> failFastGeneratedWords = new();
        Stopwatch failFastStopwatch = new();
        failFastStopwatch.Start();
        foreach (var path in pathGenerator.EnumerateAllPaths(puzzleModel.PuzzleAsAdjacencyList))
        {
            failFastGeneratedWords.Add(string.Concat(path.Select(x => x.Character)));
        }
        failFastStopwatch.Stop();
        Console.WriteLine("Time elapsed: " + failFastStopwatch.Elapsed);

        Console.WriteLine("Running model generator...");
        SolverModel solverModel = new(puzzleModel, false);
        solverModel.StateChanged += (sender, e) =>
        {
            if (e.CurrentState is not SolverCompleted)
            {
                return;
            }

            Console.WriteLine("Time elapsed: " + solverModel.TimeSpentSolving);
            //Console.WriteLine("Press any key to print generated words . . . ");
            //Console.ReadKey();
            //foreach (AnswerModel answer in solverModel.AnswersFound)
            //{
            //    Console.WriteLine(answer.Word);
            //}
        };

        await solverModel.StartSolvingPuzzle();
        HashSet<string> modelWords = solverModel.AnswersFound.Select(x => x.Word).ToHashSet();
        Console.WriteLine("Cross-Validating results...");
        int inconsistencyCount = 0;
        Console.WriteLine("Words missing from fail fast: ");
        foreach (string word in modelWords)
        {
            if (!failFastGeneratedWords.Contains(word))
            {
                inconsistencyCount++;
                Console.WriteLine(word);
            }
        }

        Console.WriteLine("Words missing from model answer: ");
        foreach (string word in failFastGeneratedWords)
        {
            if (!modelWords.Contains(word))
            {
                inconsistencyCount++;
                Console.WriteLine(word);
            }
        }

        Console.WriteLine("Cross-Validation inconsistencies: " + inconsistencyCount);
    }
}

