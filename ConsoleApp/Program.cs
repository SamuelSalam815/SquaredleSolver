// see https://github.com/dwyl/english-words for text file containing English words

using WpfSquaredleSolver.Model;

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

        PuzzleModel puzzleModel = new();
        Console.WriteLine("Loading words...");
        puzzleModel.LoadValidWords("words_alpha.txt");
        Console.WriteLine("Generating words...");

        SolverModel solverModel = new(puzzleModel, false);
        puzzleModel.PuzzleAsText = """
            PNOC
            RAHE
            GNGT
            IIHU
            """;
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

