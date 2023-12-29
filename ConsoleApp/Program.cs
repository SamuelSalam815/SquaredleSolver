// see https://github.com/dwyl/english-words for text file containing English words

using SquaredleSolver;
using SquaredleSolver.SolverStates;

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

        Console.WriteLine("Generating words...");
        SolverModel solverModel = new(puzzleModel, false);
        solverModel.StateChanged += (sender, e) =>
        {
            if (e.PreviousState is not SolverRunning)
            {
                return;
            }

            Console.WriteLine("Time taken to generate words: " + (solverModel.StopTime - solverModel.StartTime));
            Console.WriteLine("Press any key to print generated words . . . ");
            Console.ReadKey();
            foreach (AnswerModel answer in solverModel.AnswersFoundInPuzzle)
            {
                Console.WriteLine(answer.Word);
            }
        };

        await solverModel.StartSolvingPuzzle();
    }
}

